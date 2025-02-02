using doan1_v1.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace doan1_v1.Helpers
{
	public class SeederRoleUser
	{
		public static async Task Initialize(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			// lay danh sach cac class ke thua user
			var userTypes = Assembly.GetExecutingAssembly().GetTypes()
			   .Where(t => t.IsSubclassOf(typeof(User)) && !t.IsAbstract)
			   .ToList();
			foreach (var userType in userTypes)
			{
				var role = userType.Name;
				if (!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}
			}
		}
	}
}
