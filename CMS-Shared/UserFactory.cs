using CMS_DTO.Models;
using CMS_Entity;
using CMS_Entity.Entity;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CMS_Shared.Factory
{
    public class UserFactory
    {
        protected static UserFactory _instance;
        public static UserFactory Instance
        {
            get
            {
                _instance = _instance != null ? _instance : new UserFactory();
                return _instance;
            }
        }
        public LoginResponseModel Login(LoginRequestModel info)
        {
            NSLog.Logger.Info("Employee Login Start", info);
            LoginResponseModel user = null;
            try
            {
                using (CMS_Context _db = new CMS_Context())
                {
                    info.Password = CommonHelper.Encrypt(info.Password);
                    string serverImage = ConfigurationManager.AppSettings["PublicImages"];

                    var emp = _db.CMS_Employee.Where(o => o.Employee_Email == info.Email.ToLower().Trim() && o.Password == info.Password).FirstOrDefault();
                    if (emp != null)
                    {
                        user = new LoginResponseModel()
                        {
                            EmployeeID = emp.Id,
                            EmployeeName = emp.FirstName + " " + emp.LastName,
                            EmployeeEmail = emp.Employee_Email,
                            EmployeeImageURL = string.IsNullOrEmpty(emp.ImageURL) ? "" : serverImage + "Employees/" + emp.ImageURL,
                            IsSupperAdmin = emp.IsSupperAdmin,
                        };
                    }
                    NSLog.Logger.Info("Employee Login Done", user);
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("Employee Login Error", ex); }
            return user;
        }
    }
}
