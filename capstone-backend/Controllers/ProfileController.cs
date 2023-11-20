using capstone_backend.Models;
using capstone_backend.Service;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace capstone_backend.Controllers
{



    [Route("api/profile")]
    [ApiController]
    public class ProfileController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly PostRepository _postRepository;
        private readonly BcryptPasswordHasher _passwordHasher;


        public ProfileController(UserRepository userRepository, PostRepository postRepository, BcryptPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _passwordHasher = passwordHasher;

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

            /*var profileResponse = new ProfileViewResponse
            {
                UserId = existingUser.Id,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                BirthDate = existingUser.BirthDate,
                Sex = existingUser.Sex,
                PhoneNumber = existingUser.PhoneNumber,
                AboutMe = existingUser.AboutMe
            };*/

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
    }
}
