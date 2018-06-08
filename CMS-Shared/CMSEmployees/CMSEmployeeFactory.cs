using CMS_DTO.CMSEmployee;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSEmployees
{
    public class CMSEmployeeFactory
    {
        public bool CreateOrUpdate(CMS_EmployeeModels model , ref string msg)
        {
            var result = true;
            using (var cxt = new CMS_Context())
            {
                using (var trans = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        if(string.IsNullOrEmpty(model.Id))
                        {
                            var _Id = Guid.NewGuid().ToString();
                            var e = new CMS_Employee
                            {
                                Id = _Id,
                                BirthDate = model.BirthDate,
                                CreatedBy = model.CreatedBy,
                                CreatedDate = DateTime.Now,
                                Employee_Address = model.Employee_Address,
                                Employee_Email = model.Employee_Email,
                                Employee_IDCard = model.Employee_IDCard,
                                Employee_Phone = model.Employee_Phone,
                                FirstName = model.FirstName,
                                IsActive = model.IsActive,
                                LastName = model.LastName,
                                Password = model.Password,
                                UpdatedBy = model.UpdatedBy,
                                UpdatedDate = DateTime.Now,
                                ImageURL = model.ImageURL
                            };
                            cxt.CMS_Employees.Add(e);
                        }
                        else
                        {
                            var e = cxt.CMS_Employees.Find(model.Id);
                            if(e != null)
                            {
                                e.BirthDate = model.BirthDate;
                                e.UpdatedBy = model.UpdatedBy;
                                e.Employee_Address = model.Employee_Address;
                                e.Employee_Email = model.Employee_Email;
                                e.Employee_IDCard = model.Employee_IDCard;
                                e.Employee_Phone = model.Employee_Phone;
                                e.FirstName = model.FirstName;
                                e.LastName = model.LastName;
                                e.IsActive = model.IsActive;
                                e.Password = model.Password;
                                e.UpdatedDate = DateTime.Now;
                                e.ImageURL = model.ImageURL;
                            }
                        }
                        cxt.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        msg = "Vui lòng kiểm tra đường truyền";
                        result = false;
                        trans.Rollback();
                    }
                    finally
                    {
                        cxt.Dispose();
                    }
                }
            }
            return result;
        }

        public bool Delete(string Id, ref string msg)
        {
            var result = true;
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var e = cxt.CMS_Employees.Find(Id);
                    cxt.CMS_Employees.Remove(e);
                    cxt.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "Không thể xóa nhân viên này";
                result = false;
            }
            return result;
        }

        public CMS_EmployeeModels GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Employees.Where(x => x.Id.Equals(Id))
                                                .Select(x => new CMS_EmployeeModels
                                                {
                                                    Id = x.Id,
                                                    BirthDate = x.BirthDate,
                                                    CreatedBy = x.CreatedBy,
                                                    CreatedDate = x.CreatedDate,
                                                    Employee_Address = x.Employee_Address,
                                                    Employee_Email = x.Employee_Email,
                                                    Employee_IDCard = x.Employee_IDCard,
                                                    Employee_Phone = x.Employee_Phone,
                                                    FirstName = x.FirstName,
                                                    IsActive = x.IsActive,
                                                    LastName = x.LastName,
                                                    Password = x.Password,
                                                    UpdatedBy = x.UpdatedBy,
                                                    UpdatedDate = x.UpdatedDate,
                                                    ImageURL = x.ImageURL
                                                }).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception) { }
            return null;
        }

        public List<CMS_EmployeeModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Employees.Select(x => new CMS_EmployeeModels
                                                {
                                                    Id = x.Id,
                                                    BirthDate = x.BirthDate,
                                                    CreatedBy = x.CreatedBy,
                                                    CreatedDate = x.CreatedDate,
                                                    Employee_Address = x.Employee_Address,
                                                    Employee_Email = x.Employee_Email,
                                                    Employee_IDCard = x.Employee_IDCard,
                                                    Employee_Phone = x.Employee_Phone,
                                                    FirstName = x.FirstName,
                                                    IsActive = x.IsActive,
                                                    LastName = x.LastName,
                                                    Password = x.Password,
                                                    UpdatedBy = x.UpdatedBy,
                                                    UpdatedDate = x.UpdatedDate,
                                                    ImageURL = x.ImageURL
                                                }).ToList();
                    return data;
                }
            }
            catch (Exception) { }
            return null;
        }
    }
}
