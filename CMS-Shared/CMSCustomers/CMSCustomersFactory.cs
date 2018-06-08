using CMS_DTO.CMSCustomer;
using CMS_Entity;
using CMS_Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Shared.CMSCustomers
{
    public class CMSCustomersFactory
    {
        public bool InsertOrUpdate(CustomerModels model, ref string msg)
        {
            var result = true;
            using (var cxt = new CMS_Context())
            {
                using (var trans = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        if(string.IsNullOrEmpty(model.ID))
                        {
                            var e = new CMS_Customers
                            {
                                Id = Guid.NewGuid().ToString(),
                                Address = model.Address,
                                BirthDate = model.BirthDate,
                                City = model.City,
                                CompanyName = model.CompanyName,
                                Country = model.Country,
                                CreatedBy = model.CreatedBy,
                                CreatedDate = DateTime.Now,
                                Description = model.Description,
                                Email = model.Email,
                                FirstName = model.FirstName,
                                Gender = model.Gender,
                                ImageURL = model.ImageURL,
                                IsActive = model.IsActive,
                                LastName = model.LastName,
                                MaritalStatus = model.MaritalStatus,
                                Password = model.Password,
                                Phone = model.Phone,
                                Street = model.Street,
                                UpdatedBy = model.UpdatedBy,
                                UpdatedDate = DateTime.Now
                            };
                            cxt.CMS_Customers.Add(e);
                        }
                        else
                        {
                            var e = cxt.CMS_Customers.Find(model.ID);
                            if(e != null)
                            {
                                e.Address = model.Address;
                                e.BirthDate = model.BirthDate;
                                e.City = model.City;
                                e.CompanyName = model.CompanyName;
                                e.Country = model.Country;
                                e.UpdatedBy = model.UpdatedBy;
                                e.FirstName = model.FirstName;
                                e.Description = model.Description;
                                e.Email = model.Email;
                                e.Gender = model.Gender;
                                e.ImageURL = model.ImageURL;
                                e.IsActive = model.IsActive;
                                e.LastName = model.LastName;
                                e.MaritalStatus = model.MaritalStatus;
                                e.Password = model.Password;
                                e.Phone = model.Phone;
                                e.Street = model.Street;
                            }
                        }
                        cxt.SaveChanges();
                        trans.Commit();
                    }
                    catch(Exception ex) {
                        result = false;
                        trans.Rollback();
                        msg = "Lỗi đường truyền mạng";
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
            using (var cxt = new CMS_Context())
            {
                using (var trans = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        var e = cxt.CMS_Customers.Find(Id);
                        cxt.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex) {
                        result = false;
                        msg = "Không thể xóa thể loại này";
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

        public CustomerModels GetDetail(string Id)
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Customers.Where(x => x.Id.Equals(Id))
                                                .Select(x => new CustomerModels
                                                {
                                                    Address = x.Address,
                                                    BirthDate = x.BirthDate,
                                                    City = x.City,
                                                    CompanyName = x.CompanyName,
                                                    Country = x.Country,
                                                    CreatedBy = x.CreatedBy,
                                                    CreatedDate = x.CreatedDate,
                                                    Description = x.Description,
                                                    Email = x.Email,
                                                    FirstName = x.FirstName,
                                                    Gender = x.Gender,
                                                    ID = x.Id,
                                                    ImageURL = x.ImageURL,
                                                    IsActive = x.IsActive,
                                                    LastName = x.LastName,
                                                    MaritalStatus = x.MaritalStatus,
                                                    Password = x.Password,
                                                    Phone = x.Phone,
                                                    Street = x.Street,
                                                    UpdatedBy = x.UpdatedBy,
                                                    UpdatedDate = x.UpdatedDate
                                                }).FirstOrDefault();
                    return data;
                }
            }
            catch(Exception ex) { }
            return null;
        }

        public List<CustomerModels> GetList()
        {
            try
            {
                using (var cxt = new CMS_Context())
                {
                    var data = cxt.CMS_Customers.Select(x => new CustomerModels
                    {
                        Address = x.Address,
                        BirthDate = x.BirthDate,
                        City = x.City,
                        CompanyName = x.CompanyName,
                        Country = x.Country,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        Description = x.Description,
                        Email = x.Email,
                        FirstName = x.FirstName,
                        Gender = x.Gender,
                        ID = x.Id,
                        ImageURL = x.ImageURL,
                        IsActive = x.IsActive,
                        LastName = x.LastName,
                        MaritalStatus = x.MaritalStatus,
                        Password = x.Password,
                        Phone = x.Phone,
                        Street = x.Street,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate
                    }).ToList();
                    return data;
                }
            }
            catch(Exception ex) { }
            return null;
        }
    }
}
