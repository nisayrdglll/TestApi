using DataAccess.Models;
using DataModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace Core.Interfaces
{
    public interface IUserService : IServiceProvider<Aspnetusers>
    {
        string GetUserName();
        string GetName();
        Aspnetusers GetUser();
		DataResponseModel<UserViewModel> Read(AjaxRequestModel request);

	}
}
