using capstone_backend.Models;
using capstone_backend.Service;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace capstone_backend.Controllers
{

    [Route("api/profile")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly BcryptPasswordHasher _passwordHasher;
        private readonly FriendRepository _friendRepository;

        public UserController(UserRepository userRepository, BcryptPasswordHasher passwordHasher, FriendRepository friendRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _friendRepository = friendRepository;
        }


        [HttpGet("get-profile/{userId}")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_user");
            }

            User? checkingForUser = await _userRepository.GetUserById(userId);

            if(checkingForUser == null)
            {
                return BadRequest("user_DNE");
            }

            return Ok(checkingForUser);
        }

        [HttpGet("get-mini-profile/{userId}")]
        public async Task<IActionResult> GetMiniProfile(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_user");
            }

            User? checkingForUser = await _userRepository.GetUserById(userId);

            if (checkingForUser == null)
            {
                return BadRequest("user_DNE");
            }

            int friendCount = await _friendRepository.GetFriendCountByUserId(userId);

            var miniProfile = new MiniProfileDTO
            {
                FirstName = checkingForUser.FirstName,
                LastName = checkingForUser.LastName,
                Photo = checkingForUser.Photo,
                FriendCount = friendCount
            };
                               
            return Ok(miniProfile);
        }

        [HttpPut("edit-profile/{userId}")]
        public async Task<IActionResult> EditProfile(int userId, [FromBody] ProfileDTO profileDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_post");
            }

            User? existingUser = await _userRepository.GetUserById(userId);

            if (existingUser == null)
            {
                return NotFound("user_not_found");
            }

            existingUser.FirstName = profileDTO.FirstName;
            existingUser.LastName = profileDTO.LastName;
            existingUser.BirthDate = profileDTO.BirthDate;
            existingUser.Sex = profileDTO.Sex;
            existingUser.PhoneNumber = profileDTO.PhoneNumber;
            existingUser.AboutMe = profileDTO.AboutMe;
            

            _userRepository.UpdateUser(existingUser);


            return Ok(existingUser);
        }


        [HttpPut("edit-email/{userId}")]
        public async Task<IActionResult> EditEmail(int userId, [FromBody] EditEmailDTO editEmailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_user");
            }

            User? existingUser = await _userRepository.GetUserById(userId);

            if (existingUser == null)
            {
                return NotFound("user_not_found");
            }

            existingUser.Email = editEmailDTO.Email;


            _userRepository.UpdateUser(existingUser);


            return Ok(existingUser);
        }


        [HttpPut("edit-password/{userId}")]
        public async Task<IActionResult> EditPassword(int userId, [FromBody] EditPasswordDTO editPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_user");
            }

            User? existingUser = await _userRepository.GetUserById(userId);

            if (existingUser == null)
            {
                return NotFound("user_not_found");
            }

            existingUser.HashedPassword = _passwordHasher.HashPassword(editPasswordDTO.Password);


            _userRepository.UpdateUser(existingUser);


            return Ok(existingUser);
        }


        [HttpPut("edit-profile-pic/{userId}")]
        public async Task<IActionResult> EditProfilePic(int userId, [FromBody] ProfilePictureDTO profilePicDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_user");
            }

            User? existingUser = await _userRepository.GetUserById(userId);

            if (existingUser == null)
            {
                return NotFound("user_not_found");
            }

            existingUser.Photo = profilePicDTO.Photo;
            // if the following doesn't work, try:
            // existingUser.ProfileImageId = existingUser.Photo.Id;
            existingUser.ProfileImageId = profilePicDTO.ProfileImageId;


            _userRepository.UpdateUser(existingUser);


            return Ok(existingUser);
        }

        [HttpGet("search-users/{name}")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUserBySearch(string name)
        {
            List<User> matches = await _userRepository.GetUsersBySearchName(name);

            if(matches == null)
            {
                return BadRequest("no_matching_users_found");
            }

            return Ok(matches);
        
        }

        [HttpPut("edit-about-me/{userId}")]
        public async Task<IActionResult> EditAboutMe(int userId, [FromBody] AboutMeDTO aboutMeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_user");
            }

            User? existingUser = await _userRepository.GetUserById(userId);

            if (existingUser == null)
            {
                return NotFound("user_not_found");
            }

            existingUser.AboutMe = aboutMeDTO.AboutMe;


            _userRepository.UpdateUser(existingUser);


            return Ok(existingUser);
        }
}
