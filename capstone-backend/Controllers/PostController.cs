using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using capstone_backend.Data;
using capstone_backend.Models;
using capstone_backend.Service;
using NuGet.Protocol.Plugins;

namespace capstone_backend.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly PostRepository _postRepository;


        public PostController(ApplicationDbContext context, UserRepository userRepository, PostRepository postRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        [HttpPost("add-post")]
        public IActionResult AddPost([FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_post");
            }

            _postRepository.InsertPost(post);

            return Ok(new { result = "post_added_successfully" });

            
        }


        //URL should be "websitename/firstNlastNdisambiguator/timeline/update-post"?
        /*[HttpPut("update-post")]
        public async Task<IActionResult> UpdatePostByPostId(int id)
        {

        }**/


    }
}
