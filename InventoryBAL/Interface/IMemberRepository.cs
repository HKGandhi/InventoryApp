using InventoryBAL.DTO;
using InventoryDAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBAL.Interface
{
    public interface IMemberRepository
    {

        Task<Member> GetMemberByIdAsync(int memberId);
        Task<IEnumerable<Member>> GetAllMembersAsync();
        Task<Member> AddMemberAsync(Member member);
        Task<Member> UpdateMemberAsync(Member member);
        Task<bool> DeleteMemberAsync(int memberId);

        // Customize Method

        Task<APIResponse> ProcessCSVFileAsync(IFormFile file);
    }
}
