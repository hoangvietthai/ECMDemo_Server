using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public class DbDepartmentHandler : IDbDepartmentHandler
    {
        public Response<DepartmentModel> Create(DepartmentCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var last = unitOfWork.GetRepository<Department>().GetAll().OrderByDescending(c => c.DepartmentId).FirstOrDefault();

                    Department department = new Department
                    {
                        Description = createModel.Description,
                        DepartmentId = 1,
                        Name = createModel.Name,
                        IsDeleted=false,
                        LeaderId=createModel.LeaderId,
                        ParentId=createModel.ParentId
                    };


                    if (last != null) department.DepartmentId = last.DepartmentId + 1;
                    unitOfWork.GetRepository<Department>().Add(department);
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(department.DepartmentId);
                    }
                    return new Response<DepartmentModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DepartmentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DepartmentModel> Delete(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var department = unitOfWork.GetRepository<Department>().GetById(Id);
                    if (department != null)
                    {
                        
                        unitOfWork.GetRepository<Department>().Delete(department);
                        if (unitOfWork.Save() >= 1)
                        {
                            return new Response<DepartmentModel>(1, "", null);
                        }
                        return new Response<DepartmentModel>(0, "Lưu thông tin không thành công!", null);
                    }
                    else
                        return new Response<DepartmentModel>(0, "Không tìm thấy phòng ban", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DepartmentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<DepartmentDisplayModel>> GetAll()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<Department>().GetMany(u => u.IsDeleted == false)
                        .GroupJoin(unitOfWork.GetRepository<User>().GetAll(),
                        d=>d.LeaderId,
                        u=>u.UserId,
                        (d, g) => g.Select(u=> new DepartmentDisplayModel
                        {
                            DepartmentId=d.DepartmentId,
                            Description=d.Description,
                            Leader=u.FullName,
                            Name=d.Name,
                            ParentId=d.ParentId
                        }).DefaultIfEmpty(new DepartmentDisplayModel
                        {
                            DepartmentId = d.DepartmentId,
                            Description = d.Description,
                            Leader = "Chưa có trưởng phòng",
                            Name = d.Name,
                            ParentId = d.ParentId
                        })).SelectMany(g => g)
                         .OrderByDescending(u => u.DepartmentId)
                         .ToList();
                    return new Response<List<DepartmentDisplayModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DepartmentDisplayModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<DepartmentModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var doc = unitOfWork.GetRepository<Department>().GetById(Id);
                    if (doc != null) return new Response<DepartmentModel>(1, "", Ultis.ConvertSameData<DepartmentModel>(doc));
                    else
                        return new Response<DepartmentModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DepartmentModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<DepartmentModel>> GetByParentId(int ParentId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<Department>().GetMany(u => u.IsDeleted == false && u.ParentId == ParentId)
                         .Select(u => new DepartmentModel
                         {
                             DepartmentId=u.DepartmentId,
                             Description=u.Description,
                             LeaderId=u.LeaderId,
                             ParentId=u.ParentId,
                             Name = u.Name
                         })
                         .OrderByDescending(u => u.DepartmentId)
                         .ToList();
                    return new Response<List<DepartmentModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DepartmentModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<TreeNode> GetNodes()
        {
            throw new NotImplementedException();
        }

        public Response<DepartmentModel> Update(int Id, DepartmentupdateModel updateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var dir = unitOfWork.GetRepository<Department>().GetById(Id);
                    if (dir != null)
                    {
                        dir.Name = updateModel.Name;
                        dir.Description = updateModel.Description;
                        dir.ParentId = updateModel.ParentId;
                        if (updateModel.LeaderId.HasValue)
                        {
                            dir.LeaderId = updateModel.LeaderId.Value;
                            var _user = unitOfWork.GetRepository<User>().GetById(updateModel.LeaderId.Value);
                            if (_user == null) return new Response<DepartmentModel>(0, "Trưởng phòng không tồn tại", null);
                            _user.UserRoleId = 2;
                            unitOfWork.GetRepository<User>().Update(_user);
                        }
                       
                        unitOfWork.GetRepository<Department>().Update(dir);
                       
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(dir.DepartmentId);
                        }
                        return new Response<DepartmentModel>(0, "Lưu thông tin không thành công!", null);
                    }
                    else
                        return new Response<DepartmentModel>(0, "Không tìm thấy phòng ban", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DepartmentModel>(-1, ex.ToString(), null);
            }
        }

    }
}