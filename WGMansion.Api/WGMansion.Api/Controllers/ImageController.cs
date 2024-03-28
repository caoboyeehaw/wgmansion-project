using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WGMansion.Api.Models;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.Controllers
{
    [Authorize(Roles = Roles.User)]
    [ApiController]
    [Route("/v1/[controller]")]
    public class ImageController : ControllerBase
    {
        private ILog _logger = LogManager.GetLogger(typeof(ImageController));
        private readonly IImageViewModel _imageViewModel;

        public ImageController(IImageViewModel imageViewModel) 
        {
            _imageViewModel = imageViewModel;
        }

        [HttpGet]
        [Route("/getimage")]
        public async Task<ActionResult> GetImage(string id)
        {
            try
            {
                var result = await _imageViewModel.GetImage(id);
                return File(result, "image/png");
            }
            catch (Exception e) {
                _logger.Error(e.ToString());
                return BadRequest(e.ToString());
            }
        }
    }
}
