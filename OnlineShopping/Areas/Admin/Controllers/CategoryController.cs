using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Data.Repository.IRepository;
using OnlineShopping.Model;

namespace OnlineShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfwork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfwork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _unitOfwork.Category.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {//buraya bir category sınıfı gönderiyoruz. Gelen categoriyi kayıt işlemi gerçekleştiriyoruz.
            //
            if (ModelState.IsValid)
            {//kayıt işlemi gerçeklirse bize bir index dönder.
                _unitOfwork.Category.Add(category);
                _unitOfwork.Save();
                return RedirectToAction("Index");
            }
            //aksi taktirde category dönder.
            return View(category);  
        }

        public IActionResult Edit(int id)
        {
            if(id == null || id <= 0)
            {
                return NotFound();
            }
            var category = _unitOfwork.Category.GetFirstOrDefault(x => x.Id == id);
            if(category == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if(ModelState.IsValid)
            {
                _unitOfwork.Category.Update(category);
                _unitOfwork.Save();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Delete(int id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            var category = _unitOfwork.Category.GetFirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitOfwork.Category.Remove(category);
            _unitOfwork.Save();
            return RedirectToAction("Index");
        }


    }
}
