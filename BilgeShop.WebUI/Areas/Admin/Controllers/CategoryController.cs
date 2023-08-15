using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult List()
        {
            var listCategoryDtos = _categoryService.GetCategories();

            var viewModel = listCategoryDtos.Select(x => new CategoryListViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return View(viewModel);
        }

        [HttpGet] // url'den tetiklenir.
        public IActionResult New()
        {
            return View("Form" , new CategoryFormViewModel() );
            // Ekleme ve Güncelleme işlemlerini aynı form üzerinden yapacaksanız. Formu boş dahi olsa bir model ile açmalısınız.
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var updateCategoryDto = _categoryService.GetCategory(id);

            var viewModel = new CategoryFormViewModel()
            {
                Id = updateCategoryDto.Id,
                Name = updateCategoryDto.Name,
                Description = updateCategoryDto.Description
            };

            return View("form", viewModel);
        }



        [HttpPost] // form'dan tetiklenir.
        public IActionResult Save(CategoryFormViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View("form" , formData);
            }

            if(formData.Id == 0) // ekleme işlemi
            {

                var addCategoryDto = new AddCategoryDto()
                {
                    Name = formData.Name,
                    Description = formData.Description
                };
                
                var result = _categoryService.AddCategory(addCategoryDto);

                if (result)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ViewBag.ErrorMessage = "Bu isimde bir kategori zaten mevcut.";
                    return View("Form", formData);
                }

            }
            else // güncelleme işlemi
            {
                var updateCategoryDto = new UpdateCategoryDto()
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description
                };

                _categoryService.UpdateCategory(updateCategoryDto);


                return RedirectToAction("List");
            }


        }

        public IActionResult Delete(int id)
        {
            _categoryService.DeleteCategory(id);
            return RedirectToAction("List");
        }
    }
}
