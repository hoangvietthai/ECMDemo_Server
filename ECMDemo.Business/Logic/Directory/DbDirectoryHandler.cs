using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbDirectoryHandler : IDbDirectoryHandler
    {
        public Response<DirectoryModel> Create(DirectoryCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var last = unitOfWork.GetRepository<Directory>().GetAll().OrderByDescending(c => c.DirectoryId).FirstOrDefault();

                    Directory dir = new Directory
                    {
                        CreatedByUserId = createModel.CreatedByUserId,
                        CreatedOnDate = DateTime.Now,
                        DirectoryId = 1,
                        ParentId = createModel.ParentId,
                        LastModifiedByUserId = createModel.CreatedByUserId,
                        LastModifiedOnDate = DateTime.Now,
                        Name = createModel.Name,
                        IsDelete = false,
                        ModuleId=createModel.ModuleId,
                        DepartmentId=createModel.DepartmentId
                    };


                    if (last != null) dir.DirectoryId = last.DirectoryId + 1;
                    unitOfWork.GetRepository<Directory>().Add(dir);
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(dir.DirectoryId);
                    }
                    return new Response<DirectoryModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DirectoryModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DirectoryModel> Delete(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var dir = unitOfWork.GetRepository<Directory>().GetById(Id);
                    if (dir != null)
                    {
                        dir.IsDelete = true;
                        unitOfWork.GetRepository<Directory>().Update(dir);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(dir.DirectoryId);
                        }
                        return new Response<DirectoryModel>(0, "Lưu thông tin không thành công!", null);
                    }
                    else
                        return new Response<DirectoryModel>(0, "Không tìm thấy thư mục", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DirectoryModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<DirectoryModel>> GetAll()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<Directory>().GetMany(u => u.IsDelete == false)
                         .Select(u => new DirectoryModel
                         {
                             CreatedByUserId = u.CreatedByUserId,
                             CreatedOnDate = u.CreatedOnDate,
                             DirectoryId=u.DirectoryId,
                             ParentId=u.ParentId,
                             LastModifiedByUserId = u.LastModifiedByUserId,
                             LastModifiedOnDate = u.LastModifiedOnDate,
                             Name = u.Name
                         })
                         .OrderByDescending(u => u.DirectoryId)
                         .ToList();
                    return new Response<List<DirectoryModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DirectoryModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<List<DirectoryModel>> GetAll(int UserId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user == null) return new Response<List<DirectoryModel>>(0, "", null);
                    var list = unitOfWork.GetRepository<Directory>().GetMany(u => u.IsDelete == false);
                    if (user.UserRoleId>1)
                    {
                        list = list.Where(u => u.DepartmentId == user.DepartmentId);
                    }
                    var result = list
                         .Select(u => new DirectoryModel
                         {
                             CreatedByUserId = u.CreatedByUserId,
                             CreatedOnDate = u.CreatedOnDate,
                             DirectoryId = u.DirectoryId,
                             ParentId = u.ParentId,
                             LastModifiedByUserId = u.LastModifiedByUserId,
                             LastModifiedOnDate = u.LastModifiedOnDate,
                             Name = u.Name
                         })
                         .OrderByDescending(u => u.DirectoryId)
                         .ToList();
                    return new Response<List<DirectoryModel>>(1, "", result);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DirectoryModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<DirectoryModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var doc = unitOfWork.GetRepository<Directory>().GetById(Id);
                    if (doc != null) return new Response<DirectoryModel>(1, "", Ultis.ConvertSameData<DirectoryModel>(doc));
                    else
                        return new Response<DirectoryModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DirectoryModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<DirectoryModel>> GetByParentId(int ParentId,int ModuleId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<Directory>()
                        .GetMany(u => u.IsDelete == false && u.ParentId==ParentId && u.ModuleId== ModuleId)
                         .Select(u => new DirectoryModel
                         {
                             CreatedByUserId = u.CreatedByUserId,
                             CreatedOnDate = u.CreatedOnDate,
                             DirectoryId = u.DirectoryId,
                             ParentId = u.ParentId,
                             LastModifiedByUserId = u.LastModifiedByUserId,
                             LastModifiedOnDate = u.LastModifiedOnDate,
                             Name = u.Name
                             
                         })
                         .OrderByDescending(u => u.DirectoryId)
                         .ToList();
                    return new Response<List<DirectoryModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DirectoryModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<List<DirectoryNodeModel>> GetNodes(int ModuleId,int UserId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user == null) return new Response<List<DirectoryNodeModel>>(0, "", null);
                    var list = unitOfWork.GetRepository<Directory>().GetMany(u => u.IsDelete == false && u.ModuleId == ModuleId);
                    if (user.UserRoleId > 1)
                    {
                        list = list.Where(c => c.DepartmentId == user.DepartmentId);
                    }
                    var result = list
                     .Select(u => new DirectoryNodeModel
                     {
                         data = u.DirectoryId,
                         ParentId = u.ParentId,
                         label = u.Name,
                         expandedIcon = "fa fa-folder-open",
                         collapsedIcon = "fa fa-folder"
                     }).ToList();

                    var nlist = FlatToHierarchy(result);
                    return new Response<List<DirectoryNodeModel>>(1, "", nlist.ToList());
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DirectoryNodeModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<DirectoryModel> Update(int Id, DirectoryUpdateModel updateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var dir = unitOfWork.GetRepository<Directory>().GetById(Id);
                    if (dir != null)
                    {
                        Ultis.TransferValues(dir, updateModel);
                        dir.Name = updateModel.Name;
                        dir.ParentId = updateModel.ParentId;
                        dir.LastModifiedByUserId = updateModel.LastModifiedByUserId;
                        dir.LastModifiedOnDate = DateTime.Now;
                        unitOfWork.GetRepository<Directory>().Update(dir);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(dir.DirectoryId);
                        }
                        return new Response<DirectoryModel>(0, "Lưu thông tin không thành công!", null);
                    }
                    else
                        return new Response<DirectoryModel>(0, "Không tìm thấy thư mục", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DirectoryModel>(-1, ex.ToString(), null);
            }
        }
        public IEnumerable<DirectoryNodeModel> FlatToHierarchy(List<DirectoryNodeModel> list)
        {
            // hashtable lookup that allows us to grab references to containers based on id
            var lookup = new Dictionary<int, DirectoryNodeModel>();
            // actual nested collection to return
            var nested = new List<DirectoryNodeModel>();

            foreach (DirectoryNodeModel item in list)
            {
                if (lookup.ContainsKey(item.ParentId))
                {
                    // add to the parent's child list 
                    lookup[item.ParentId].children.Add(item);
                }
                else
                {
                    // no parent added yet (or this is the first time)
                    nested.Add(item);
                }
                lookup.Add(item.data, item);
            }

            return nested;
        }

        public Response<List<DirectoryModel>> GetAllByModule(int moduleid)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<Directory>().GetMany(u => u.IsDelete == false &&u.ModuleId==moduleid)
                         .Select(u => new DirectoryModel
                         {
                             CreatedByUserId = u.CreatedByUserId,
                             CreatedOnDate = u.CreatedOnDate,
                             DirectoryId = u.DirectoryId,
                             ParentId = u.ParentId,
                             LastModifiedByUserId = u.LastModifiedByUserId,
                             LastModifiedOnDate = u.LastModifiedOnDate,
                             Name = u.Name
                         })
                         .OrderByDescending(u => u.DirectoryId)
                         .ToList();
                    return new Response<List<DirectoryModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DirectoryModel>>(-1, ex.ToString(), null);
            }
        }
    }
}