using InventoryBAL.DTO;
using InventoryBAL.Interface;
using InventoryDAL;
using InventoryDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBAL.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DBContext _context;

        public MemberRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<Member> GetMemberByIdAsync(int memberId)
        {
            return await _context.Members.FindAsync(memberId);
        }

        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _context.Members.ToListAsync();
        }

        public async Task<Member> AddMemberAsync(Member member)
        {
            var result = await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Member> UpdateMemberAsync(Member member)
        {
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<bool> DeleteMemberAsync(int memberId)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null) return false;

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<APIResponse> ProcessCSVFileAsync(IFormFile file)
        {
            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    // Optional: Skip the header if the file has a header row
                    csv.Read();
                    csv.ReadHeader();

                    // Read each record in the CSV file
                    while (csv.Read())
                    {
                        var members = new Member
                        {
                            FirstName = csv.GetField<string>("FirstName"),
                            JoiningDate = string.IsNullOrEmpty(csv.GetField<string>("JoiningDate")) ? DateTime.Now : Convert.ToDateTime(csv.GetField<string>("JoiningDate")),
                            LastName = csv.GetField<string>("LastName"),
                            BookingCount = csv.GetField<int>("BookingCount")
                        };


                        await _context.Members.AddAsync(members);
                    }


                    await _context.SaveChangesAsync();
                }
                return new APIResponse { IsSuccess = true, Message = "CSV File Data Inserted Successfully" };

            }
            catch (Exception ex)
            {
                return new APIResponse { IsSuccess = false, Message = ex.Message };
            }
        }
    }
    
}
