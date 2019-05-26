using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public class DbDocumentGroupCateHandler : IDbDocumentGroupCateHandler
    {
        public Response<DocumentGroupCateModel> ChangeActiveGroup(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var current_active = unitOfWork.GetRepository<DocumentCategoryGroup>().Get(g => g.Active);
                    if (current_active !=null)
                    {
                        if (current_active.DocumentCateGroupId == Id)
                        {
                            return new Response<DocumentGroupCateModel>(1, "", null);
                        }
                        else
                        {
                            current_active.Active = false;
                            unitOfWork.GetRepository<DocumentCategoryGroup>().Update(current_active);
                        }
                    }
                    var entity = unitOfWork.GetRepository<DocumentCategoryGroup>().GetById(Id);
                    if (entity != null)
                    {
                        entity.Active = true;
                        unitOfWork.GetRepository<DocumentCategoryGroup>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(Id);
                        }
                    }
                    return new Response<DocumentGroupCateModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentGroupCateModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentGroupCateModel> GetActiveGroup()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var cate = unitOfWork.GetRepository<DocumentCategoryGroup>().Get(g => g.Active);
                    if (cate != null) return new Response<DocumentGroupCateModel>(1, "", Ultis.ConvertSameData<DocumentGroupCateModel>(cate));
                    else
                        return new Response<DocumentGroupCateModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentGroupCateModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentGroupCateModel> Create(DocumentGroupCateCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var last = unitOfWork.GetRepository<DocumentCategoryGroup>().GetAll().OrderByDescending(c => c.DocumentCateGroupId).FirstOrDefault();

                    DocumentCategoryGroup group = new DocumentCategoryGroup
                    {
                        DocumentCateGroupId = 1,
                        Name = createModel.Name
                    };


                    if (last != null) group.DocumentCateGroupId = last.DocumentCateGroupId + 1;
                    unitOfWork.GetRepository<DocumentCategoryGroup>().Add(group);
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(group.DocumentCateGroupId);
                    }
                    return new Response<DocumentGroupCateModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentGroupCateModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentGroupCateModel> Delete(int Id)
        {
            try
            {
                using (var unitOfWork=new UnitOfWork())
                {
                    var entity=unitOfWork.GetRepository<DocumentCategoryGroup>().GetById(Id);
                    if (entity != null)
                    {
                        foreach(var item in unitOfWork.GetRepository<DocumentCategory>().GetMany(c => c.CategoryGroupId == Id))
                        {
                            unitOfWork.GetRepository<DocumentCategory>().Delete(item);
                        }
                        unitOfWork.GetRepository<DocumentCategoryGroup>().Delete(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return new Response<DocumentGroupCateModel>(1, "", null);
                        }
                    }
                    return new Response<DocumentGroupCateModel>(0, "Không tìm thấy nhóm thể loại", null);
                }
                
            }
            catch(Exception ex)
            {
                return new Response<DocumentGroupCateModel>(-1, ex.ToString(), null);
            }
        }

       

        public Response<List<DocumentGroupCateModel>> GetAll()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<DocumentCategoryGroup>().GetAll()
                         .Select(u => new DocumentGroupCateModel
                         {
                             DocumentCateGroupId = u.DocumentCateGroupId,
                             Name = u.Name
                         })
                         .OrderBy(u => u.DocumentCateGroupId)
                         .ToList();
                    return new Response<List<DocumentGroupCateModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentGroupCateModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentGroupCateModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var cate = unitOfWork.GetRepository<DocumentCategoryGroup>().GetById(Id);
                    if (cate != null) return new Response<DocumentGroupCateModel>(1, "", Ultis.ConvertSameData<DocumentGroupCateModel>(cate));
                    else
                        return new Response<DocumentGroupCateModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentGroupCateModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentGroupCateDetailModel> GetDetail(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    DocumentGroupCateDetailModel result = new DocumentGroupCateDetailModel();
                    var cate = unitOfWork.GetRepository<DocumentCategoryGroup>().GetById(Id);
                    if (cate != null)
                    {
                        result.DocumentCateGroupId = cate.DocumentCateGroupId;
                        result.Name = cate.Name;
                        result.Categories = new List<DocumentCategoryModel>();
                        var list = unitOfWork.GetRepository<DocumentCategory>().GetMany(c => c.CategoryGroupId == Id)
                         .Select(u => new DocumentCategoryModel
                         {
                             CategoryId = u.CategoryId,
                             Description = u.Description,
                             Name = u.Name
                         })
                         .OrderBy(u => u.Name)
                         .ToList();
                        if(list.Count>0)
                        result.Categories = list;
                        return new Response<DocumentGroupCateDetailModel>(1, "", result);
                    }
                    return new Response<DocumentGroupCateDetailModel>(0, "Not found", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentGroupCateDetailModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentGroupCateModel> Update(int Id, DocumentGroupCateUpdateModel updateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentCategoryGroup>().GetById(Id);

                    entity.Name = updateModel.Name;


                   
                    unitOfWork.GetRepository<DocumentCategoryGroup>().Update(entity);
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(Id);
                    }
                    return new Response<DocumentGroupCateModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentGroupCateModel>(-1, ex.ToString(), null);
            }
        }
    }
}