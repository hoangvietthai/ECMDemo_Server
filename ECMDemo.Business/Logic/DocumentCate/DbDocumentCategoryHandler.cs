using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMDemo.Business.Handler
{
    public class DbDocumentCategoryHandler : IDbDocumentCategoryHandler
    {
        public Response<DocumentCategoryModel> Create(DocumentCategoryCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var last = unitOfWork.GetRepository<DocumentCategory>().GetAll().OrderByDescending(c => c.CategoryId).FirstOrDefault();

                    DocumentCategory cate = new DocumentCategory
                    {
                       CategoryId=1,
                       Description=createModel.Description,
                       Name=createModel.Name,
                       CategoryGroupId=createModel.CategoryGroupId
                    };


                    if (last != null) cate.CategoryId = last.CategoryId + 1;
                    unitOfWork.GetRepository<DocumentCategory>().Add(cate);
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(cate.CategoryId);
                    }
                    return new Response<DocumentCategoryModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentCategoryModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentCategoryModel> Delete(int Id)
        {
            try
            {
                using (var unitOfWork=new UnitOfWork())
                {
                    var entity = unitOfWork.GetRepository<DocumentCategory>().GetById(Id);
                    if (entity != null)
                    {
                        unitOfWork.GetRepository<DocumentCategory>().Delete(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return new Response<DocumentCategoryModel>(1, "", new DocumentCategoryModel
                            {
                                CategoryGroupId = Id,
                                Name = entity.Name
                            });
                        }
                    }
                    return new Response<DocumentCategoryModel>(0, "Id is not valid", null);
                    
                }
            }
            catch(Exception ex)
            {
                return new Response<DocumentCategoryModel>(-1, ex.ToString(), null);
            }
        }

        public Response<List<DocumentCategoryModel>> GetAll()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<DocumentCategory>().GetAll()
                         .Select(u => new DocumentCategoryModel
                         {
                             CategoryId=u.CategoryId,
                             Description=u.Description,
                             Name=u.Name,
                             CategoryGroupId=u.CategoryGroupId
                         })
                         .OrderBy(u => u.CategoryGroupId)
                         .ToList();
                    return new Response<List<DocumentCategoryModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentCategoryModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<List<DocumentCategoryModel>> GetAll(int groupId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<DocumentCategory>().GetMany(c=>c.CategoryGroupId==groupId)
                         .Select(u => new DocumentCategoryModel
                         {
                             CategoryId = u.CategoryId,
                             Description = u.Description,
                             Name = u.Name
                         })
                         .OrderBy(u => u.CategoryId)
                         .ToList();
                    return new Response<List<DocumentCategoryModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentCategoryModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentCategoryModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var cate = unitOfWork.GetRepository<DocumentCategory>().GetById(Id);
                    if (cate != null) return new Response<DocumentCategoryModel>(1, "", Ultis.ConvertSameData<DocumentCategoryModel>(cate));
                    else
                        return new Response<DocumentCategoryModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentCategoryModel>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentCategoryModel> Update(int Id, DocumentCategoryUpdateModel updateModel)
        {
            try
            {
                using (var unitOfWork=new UnitOfWork())
                {
                    var entity=unitOfWork.GetRepository<DocumentCategory>().GetById(Id);
                    if (entity != null)
                    {
                        entity.Description = updateModel.Description;
                        entity.Name = updateModel.Name;
                        unitOfWork.GetRepository<DocumentCategory>().Update(entity);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(Id);
                        }
                    }
                    return new Response<DocumentCategoryModel>(0, "Id is not valid", null);
                }
            }
            catch(Exception ex)
            {
                return new Response<DocumentCategoryModel>(-1, ex.ToString(), null);
            }
        }
    }
}