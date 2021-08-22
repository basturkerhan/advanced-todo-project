using AdvancedTodoApplication.Infrastructure;
using AdvancedTodoApplication.Models;
using AdvancedTodoApplication.Repository;
using AdvancedTodoApplication.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly IBoardRepository _boardRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserService _userService;
        private readonly ToDoContext _context;
        private readonly IToDoRepository _toDoRepository;
        public ToDoController( IBoardRepository boardRepository,
                               ICategoryRepository categoryRepository,
                               IUserService userService,
                               ToDoContext context,
                               IToDoRepository toDoRepository )
        {
            _boardRepository = boardRepository;
            _categoryRepository = categoryRepository;
            _userService = userService;
            _context = context;
            _toDoRepository = toDoRepository;

        }

        // CREATE -> GET AND POST
        public async Task<IActionResult> Create(int categoryid, int boardid)
        {
            Board board = await _context.Board.FindAsync(boardid);
            Category category = await _context.Category.FindAsync(categoryid);

            // eklenecek kategori veya pano var mı kontrolü
            if (board == null || category == null)
            {
                TempData["error"] = "Böyle bir pano veya kategori yok";
                return RedirectToAction("Index", "Home");
            }
            // kullanıcı o panoya üye mi kontrolü
            bool isMember = await _boardRepository.IsUserMemberBoard(boardid);
            if (!isMember)
            {
                TempData["error"] = "Bu panonun bir üyesi değilsiniz";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.categoryid = categoryid;
            ViewBag.boardid = boardid;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ToDo item, int categoryid, int boardid)
        {
            // eklenecek kategorisi o panoya ait mi kontrolü
            bool isBoardCategoryOwner = await _boardRepository.IsCategoryOwner(categoryid, boardid);
            if (ModelState.IsValid && isBoardCategoryOwner)
            {
                // kullanıcı panoya üye mi kontrolü
                string userId = _userService.GetUserId();
                bool isUserMemberBoard = await _boardRepository.IsUserMemberBoard(userId, boardid);
                if (!isUserMemberBoard)
                {
                    TempData["error"] = "Bu panonun bir üyesi değilsiniz";
                    return RedirectToAction("Index", "Home");
                }

                item.OwnerId = userId;
                item.CreatedAt = DateTime.Now;
                ApplicationUser user = await _userService.GetUserById(userId);

                // many-to-many ilişkisi için gereken nesne
                UserToDo userToDo = new UserToDo()
                {
                    UserId = _userService.GetUserId(),
                    ToDoId = item.Id,
                    ToDo = item,
                    ApplicationUser = user
                };

                // bu alt kısmı modelde halledeceğim burası karışmasın
                item.ToDoUsers = new List<UserToDo>() { };
                item.ToDoUsers.Add(userToDo);

                // ilgili kategoriye yeni todo yu ekleme işlemi
                Category category = await _categoryRepository.GetCategoryById(categoryid);
                category.Todos.Add(item);

                // todo nesnesini veritabanına ekleme ve veritabanını güncelleme
                _context.ToDo.Add(item);
                _context.Category.Update(category);
                await _context.SaveChangesAsync();

                TempData["success"] = "İş kartı başarıyla oluşturuldu";
                return RedirectToAction("Details", "Board", new { id = boardid });
            }

            return View(item);
        }


        // DELETE -> POST
        public async Task<IActionResult> Delete(int id, int boardid)
        {
            // kullanıcı panoya üye mi kontrolü
            bool isUserMemberBoard = await _boardRepository.IsUserMemberBoard(boardid);
            if (!isUserMemberBoard)
            {
                TempData["error"] = "Bu panonun bir üyesi değilsiniz";
                return RedirectToAction("Index", "Home");
            }

            // kartı silecek kişi kartta ekli mi kontrolü
            bool isUserMemberToDo = await _toDoRepository.IsUserMemberToDo(id);
            if (!isUserMemberToDo)
            {
                TempData["error"] = "Bu iş kartının bir üyesi değilsiniz";
                return RedirectToAction("Details", "Board", new { id = boardid });
            }

            bool isDeleted = await _toDoRepository.DeleteToDo(id);
            if (!isDeleted)
            {
                TempData["error"] = "Böyle bir iş kartı mevcut değil";
            }
            else
            {
                TempData["success"] = "İş kartı başarıyla kaldırıldı";
            }

            return RedirectToAction("Details", "Board", new { id = boardid });
        }


        // EDIT -> GET AND POST
        public async Task<IActionResult> Edit(int todoid, int boardid)
        {
            ToDo todo = await _context.ToDo.FindAsync(todoid);

            if (todo == null)
            {
                return NotFound();
            }

            // kartı düzenleyecek kişi kartta ekli mi kontrolü
            bool isUserMemberToDo = await _toDoRepository.IsUserMemberToDo(todoid);
            if (!isUserMemberToDo)
            {
                TempData["error"] = "Bu iş kartının bir üyesi değilsiniz";
                return RedirectToAction("Details", "Board", new { id = boardid });
            }

            ViewBag.boardid = boardid;

            return View(todo);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ToDo item, int boardid)
        {
            if (ModelState.IsValid)
            {
                // kullanıcı panoya üye mi kontrolü
                bool isUserMemberBoard = await _boardRepository.IsUserMemberBoard(boardid);
                if (!isUserMemberBoard)
                {
                    TempData["error"] = "Bu panonun bir üyesi değilsiniz";
                    return RedirectToAction("Index", "Home");
                }

                // kartı düzenleyecek kişi kartta ekli mi kontrolü
                bool isUserMemberToDo = await _toDoRepository.IsUserMemberToDo(item.Id);
                if (!isUserMemberToDo)
                {
                    TempData["error"] = "Bu iş kartının bir üyesi değilsiniz";
                    return RedirectToAction("Details", "Board", new { id = boardid });
                }

                _context.Update(item);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Board", new { id = boardid });
            }
            return View(item);
        }


        [HttpPost]
        public async Task<bool> AddUserToToDo(string userid, int todoid, int boardid)
        {
            // karta kullanıcı ekleyecek kişi kartta ekli mi kontrolü
            bool isUserMemberToDo = await _toDoRepository.IsUserMemberToDo(todoid);
            if (!isUserMemberToDo)
            {
                return false;
            }

            // karta eklenecek kişi o panoda var mı kontrolü
            bool isUserMemberBoard = await _boardRepository.IsUserMemberBoard(userid, boardid);
            if(!isUserMemberBoard)
            {
                return false;
            }

            // eklenecek kullanıcı zaten kartta ekli mi kontrolü
            bool isUserAlreadyMemberToDo = await _toDoRepository.IsUserMemberToDo(userid, todoid);
            if(isUserAlreadyMemberToDo)
            {
                return false;
            }

            return await _toDoRepository.AddUserToToDo(userid, todoid);
        }


        [HttpPost]
        public async Task<bool> RemoveUserFromToDo(string userid, int todoid)
        {
            // karttan kullanıcı silecek kişi kartta ekli mi kontrolü
            bool isUserMemberToDo = await _toDoRepository.IsUserMemberToDo(todoid);
            if (!isUserMemberToDo)
            {
                return false;
            }

            return await _toDoRepository.RemoveUserFromToDo(userid, todoid);
        }


        [HttpPost]
        public async Task<bool> CheckThisToDo(int todoId)
        {
            bool isUserMemberToDo = await _toDoRepository.IsUserMemberToDo(todoId);
            if( !isUserMemberToDo )
            {
                return false;
            }

            ToDo todo = await _toDoRepository.GetToDoById(todoId);
            todo.IsChecked = !todo.IsChecked;

            DateTime? newdate = null;
            todo.FinishedAt = todo.IsChecked ? DateTime.Now : newdate;

            _context.ToDo.Update(todo);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
