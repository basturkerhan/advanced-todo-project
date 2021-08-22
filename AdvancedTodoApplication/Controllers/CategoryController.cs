using AdvancedTodoApplication.Infrastructure;
using AdvancedTodoApplication.Models;
using AdvancedTodoApplication.Repository;
using AdvancedTodoApplication.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ToDoContext _context;
        private readonly IBoardRepository _boardRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserService _userService;

        public CategoryController(  ToDoContext context, 
                                    IBoardRepository boardRepository, 
                                    ICategoryRepository categoryRepository,
                                    IUserService userService  )
        {
            _context = context;
            _boardRepository = boardRepository;
            _categoryRepository = categoryRepository;
            _userService = userService;
        }


        // CREATE -> GET AND POST
        public IActionResult Create(int eklenecekpano)
        {
            ViewBag.eklenecekpano = eklenecekpano;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category item, int eklenecekpano)
        {
            if (ModelState.IsValid)
            {
                // kullanıcı bu panoda yoksa bu pano için kategori oluşturamaz
                bool isUserMemberBoard = await _boardRepository.IsUserMemberBoard(eklenecekpano);
                if (!isUserMemberBoard)
                {
                    TempData["error"] = "Bu panonun bir üyesi değilsiniz";
                    return RedirectToAction("Index", "Home");
                }
                // bitiş

                Board board = await _boardRepository.GetBoardById(eklenecekpano);

                board.BoardCategories.Add(item);
                _context.Category.Add(item);
                _context.Board.Update(board);

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Board", new { id = eklenecekpano });
            }
            return View(item);
        }


        // DELETE -> POST
        public async Task<IActionResult> Delete(int id, int boardid)
        {
            // sadece pano sahibi kategorileri silebilir
            bool isUserAdminBoard = await _boardRepository.IsUserAdminBoard(boardid);
            if (!isUserAdminBoard)
            {
                TempData["error"] = "Kategorileri yalnızca pano sahibi kaldırabilir";
                return RedirectToAction("Details", "Board", new { id = boardid });
            }

            bool isDeleted = await _categoryRepository.DeleteCategory(id);

            if (!isDeleted)
            {
                TempData["error"] = "Böyle bir kategori mevcut değil";
            }
            else
            {
                TempData["success"] = "Kategori başarıyla kaldırıldı";
            }

            return RedirectToAction("Details", "Board", new { id = boardid });
        }



        // EDIT -> GET AND POST
        public async Task<IActionResult> Edit(int id, int boardid)
        {
            // kullanıcı panoya üye mi kontrolü
            bool isUserMemberBoard = await _boardRepository.IsUserMemberBoard(boardid);
            if (!isUserMemberBoard)
            {
                TempData["error"] = "Bu panonun bir üyesi değilsiniz";
                return RedirectToAction("Index", "Home");
            }

            Category category = await _context.Category.FindAsync(id);
            if(category == null)
            {
                return NotFound();
            }
            ViewBag.boardid = boardid;

            return View(category);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Category item, int boardid)
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

                _context.Category.Update(item);
                await _context.SaveChangesAsync();
                TempData["success"] = "Kategori başarıyla güncellendi";
                return RedirectToAction("Details", "Board", new { id = boardid });
            }
            return View(item);
        }


    }
}
