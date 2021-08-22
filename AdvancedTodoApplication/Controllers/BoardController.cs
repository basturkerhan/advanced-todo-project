using AdvancedTodoApplication.Infrastructure;
using AdvancedTodoApplication.Models;
using AdvancedTodoApplication.Repository;
using AdvancedTodoApplication.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        private readonly IUserService _userService;
        private readonly ToDoContext _context;
        private readonly IBoardRepository _boardRepository = null;
        private readonly ICategoryRepository _categoryRepository;

        public BoardController( IUserService userService, 
                                ToDoContext context, 
                                IBoardRepository boardRepository, 
                                ICategoryRepository categoryRepository )
        {
            _userService = userService;
            _context = context;
            _boardRepository = boardRepository;
            _categoryRepository = categoryRepository;
        }


        [Route("my-boards")]
        public async Task<IActionResult> Index()
        {
            bool isLoggedIn = _userService.IsAuthenticated();
            string userId = _userService.GetUserId();

            IQueryable<Board> items = from b in _context.UserBoard where b.UserId==userId select b.Board;
            List<Board> boardList = await items.ToListAsync();

            return View(boardList);
        }


        // EDIT - GET AND POST
        public async Task<IActionResult> Edit(int id)
        {
            Board board = await _context.Board.FindAsync(id);

            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        // burası tamam
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Board item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Board", new { id = item.Id });
            }

            return View(item);
        }


        // CREATE - GET AND POST
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Board item)
        {
            if (ModelState.IsValid)
            {
                string userId = _userService.GetUserId();
                item.OwnerId = userId;

                ApplicationUser user = await _userService.GetUserById(userId);
                UserBoard userBoard = new UserBoard() 
                { 
                    BoardId = item.Id,
                    Board = item,
                    UserId = userId,
                    ApplicationUser = user
                };

                item.BoardCategories = new List<Category>() { };
                item.UserBoards = new List<UserBoard>() { };

                item.UserBoards.Add(userBoard);
                _context.Board.Add(item);

                await _context.SaveChangesAsync();
                TempData["success"] = "Pano başarıyla oluşturuldu";

                return RedirectToAction("Index", "Board");
            }

            return View(item);
        }


        [Route("board-details/{id:int:min(1)}", Name = "boardDetailsRoute")]
        public async Task<IActionResult> Details(int id)
        {
            string userId = _userService.GetUserId();
            bool isMember = await _boardRepository.IsUserMemberBoard(userId, id);

            if (!isMember)
            {
                return RedirectToAction("Index", "Board");
            }

            ViewBag.userId = userId;
            var board = await _boardRepository.GetBoardById(id);

            return View(board);
        }


        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await _boardRepository.DeleteBoard(id);

            if (!isDeleted)
            {
                TempData["error"] = "Böyle bir pano mevcut değil";
            }
            else
            {
                TempData["success"] = "Pano başarıyla kaldırıldı / üyelikten çıkıldı";
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<bool> AddUserToBoard(string userid, int boardid)
        {
            return await _boardRepository.AddUserToBoard(userid, boardid);
        }

        [HttpPost]
        // burası tamam
        public async Task<bool> RemoveUserFromBoard(string userid, int boardid)
        {
            return await _boardRepository.RemoveUserFromBoard(userid, boardid);
        }


    }
}
