using CMS_DTO.CMSCrawler;
using CMS_DTO.CMSEmployee;
using CMS_Entity;
using CMS_Entity.Entity;
using CMS_Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMS_Shared.CMSEmployees
{
    public class CMSPinFactory
    {
        private static Semaphore m_Semaphore = new Semaphore(1, 1); /* semaphore for create pin data */

        public bool CreateOrUpdate(List<PinsModels> lstPin, string KeyWordID, string createdBy, ref string msg)
        {
            var result = true;

            m_Semaphore.WaitOne();
            try
            {
                using (var _db = new CMS_Context())
                {
                    var lstPinID = lstPin.Select(o => o.ID).ToList();
                    var lstPinUpdate = _db.CMS_Pin.Where(o => lstPinID.Contains(o.ID)).ToList();
                    var lstPinUpdateID = lstPinUpdate.Select(o => o.ID).ToList();
                    var lstPinInsert = lstPin.Where(o => !lstPinUpdateID.Contains(o.ID)).ToList();

                    /* update pin */
                    foreach (var uPin in lstPinUpdate)
                    {
                        uPin.Repin_count = lstPin.Where(o => o.ID == uPin.ID).Select(o => o.Repin_count).FirstOrDefault();
                        uPin.UpdatedBy = createdBy;
                        uPin.UpdatedDate = DateTime.Now;
                    }

                    /* insert new pin */
                    var listInsertDB = new List<CMS_Pin>();
                    foreach (var pin in lstPinInsert)
                    {
                        listInsertDB.Add(new CMS_Pin()
                        {
                            ID = pin.ID,
                            Link = pin.Link,
                            Repin_count = pin.Repin_count,
                            ImageUrl = pin.Images.Select(o => o.url).First(),
                            Created_At = pin.Created_At,
                            Status = (byte)Commons.EStatus.Active,
                            CreatedBy = createdBy,
                            CreatedDate = DateTime.Now,
                            UpdatedBy = createdBy,
                            UpdatedDate = DateTime.Now,
                        });
                    }
                    if (listInsertDB.Count > 0)
                        _db.CMS_Pin.AddRange(listInsertDB);

                    /* TABLE KEYWORD_PIN */
                    var lstKeyWrd_Pin_Exist = _db.CMS_R_KeyWord_Pin.Where(o => o.KeyWordID == KeyWordID && lstPinID.Contains(o.PinID)).Select(o => o.PinID).ToList();
                    var lstKeyWrd_Pin_New = lstPinID.Where(o => !lstKeyWrd_Pin_Exist.Contains(o)).ToList();
                    var lstKeyWrd_Pin_InsertBD = lstKeyWrd_Pin_New.Select(o => new CMS_R_KeyWord_Pin()
                    {
                        ID = Guid.NewGuid().ToString(),
                        KeyWordID = KeyWordID,
                        PinID = o,
                        Status = (byte)Commons.EStatus.Active,
                        CreatedBy = createdBy,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = createdBy,
                        UpdatedDate = DateTime.Now,
                    }).ToList();

                    if (lstKeyWrd_Pin_InsertBD.Count > 0)
                        _db.CMS_R_KeyWord_Pin.AddRange(lstKeyWrd_Pin_InsertBD);


                    // save db 
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = "CreateOrUpdate Pin with exception.";
                result = false;
            }

            finally
            {
                m_Semaphore.Release();
            }
            return result;
        }

        public bool GetPin(ref List<PinsModels> lstPins, List<string> lstKeyWordID, ref string msg)
        {
            var result = true;
            lstPins = new List<PinsModels>();

            try
            {
                using (var _db = new CMS_Context())
                {
                    lstPins = _db.CMS_R_KeyWord_Pin.Where(o => lstKeyWordID.Contains(o.KeyWordID))
                            .Join(_db.CMS_Pin, kp => kp.PinID, p => p.ID, (kp, p) => p)
                            .Select(o => new PinsModels()
                            {
                                ID = o.ID,
                                Link = o.Link,
                                Domain = o.Domain,
                                Repin_count = o.Repin_count ?? 0,
                                Images = new List<ImageModels>()
                                {
                                    new ImageModels()
                                    {
                                        url = o.ImageUrl,
                                    },
                                },
                                Created_At = o.Created_At ?? DateTime.MinValue,
                                CreatedDate = o.CreatedDate ?? DateTime.MinValue,
                                UpdateDate = o.UpdatedDate ?? DateTime.MinValue,
                                LastTime = CommonHelper.GetDurationFromNow(o.UpdatedDate),
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                msg = "CreateOrUpdate Pin with exception.";
                result = false;
            }
            finally
            {
            }
            return result;
        }
    }
}
