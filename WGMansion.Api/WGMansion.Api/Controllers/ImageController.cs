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
        public Func<string> GetUserId;

        public ImageController(IImageViewModel imageViewModel) 
        {
            _imageViewModel = imageViewModel;
            GetUserId = () => User.Identity.Name;
        }

        [HttpGet]
        [Route("/getimage")]
        public async Task<ActionResult<byte[]>> GetImage(string id)
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

        [Authorize(Roles=Roles.Admin)]
        [HttpPost]
        [Route("/postimage")]
        public async Task<ActionResult<string>> PostImage(IFormFile image)
        {
            try
            {
                var result = await _imageViewModel.PostImage(image, GetUserId());
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                return BadRequest(e.ToString());
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete]
        [Route("/deleteimage")]
        public async Task<ActionResult<string>> DeleteImage(string id)
        {
            try
            {
                await _imageViewModel.DeleteImage(id);
                return Ok($"Image {id} deleted");
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                return BadRequest(e.ToString());
            }
        }
    }
}
