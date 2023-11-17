using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using capstone_backend.Data;
using capstone_backend.Models;
using capstone_backend.Service;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace capstone_backend.Controllers
{
    [Route("api/timeline")]
    [ApiController]
    public class TimelineController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly TimelineRepository _timelineRepository;


        public TimelineController(ApplicationDbContext context, UserRepository userRepository, TimelineRepository timelineRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _timelineRepository = timelineRepository;
        }


        // This gets all the posts under a single user's timeline
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAllPosts(int userId)
        { 
            int timelineId = _timelineRepository.GetTimelineByUserId(userId).Id;

            var posts = _timelineRepository.GetPostsByTimelineId(timelineId);

            return posts;
        }


        

    }


}
