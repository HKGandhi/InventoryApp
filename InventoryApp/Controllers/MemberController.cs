using InventoryBAL.DTO;
using InventoryBAL.Interface;
using InventoryBAL.Repository;
using InventoryDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;

        public MemberController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetAllMembers()
        {
            var members = await _memberRepository.GetAllMembersAsync();
            return Ok(members);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMemberById(int id)
        {
            var member = await _memberRepository.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }

        
        [HttpPost]
        public async Task<ActionResult<Member>> CreateMember([FromBody] Member member)
        {
            var createdMember = await _memberRepository.AddMemberAsync(member);
            return CreatedAtAction(nameof(GetMemberById), new { id = createdMember.Id }, createdMember);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] Member member)
        {
            if (id != member.Id)
            {
                return BadRequest();
            }

            var updatedMember = await _memberRepository.UpdateMemberAsync(member);
            return Ok(updatedMember);
        }

    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var deleted = await _memberRepository.DeleteMemberAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }


        // CSV Upload 

        [HttpPost("UploadInventoryCSV")]
        public async Task<APIResponse> UploadInventoryCSV(IFormFile file)
        {

            return await _memberRepository.ProcessCSVFileAsync(file);

        }
    }
}
