using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECMDemo.Business.Common;
using ECMDemo.Business.Model;
using ECMDemo.Data;

namespace ECMDemo.Business.Handler
{
    public class DbDocumentHandler : IDbDocumentHandler
    {
        public Response<DocumentModel> Create(int UserId,DocumentCreateModel createModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user==null) return new Response<DocumentModel>(0, "", null);
                    var last = unitOfWork.GetRepository<Document>().GetAll().OrderByDescending(c => c.DocumentId).FirstOrDefault();

                    Document doc = new Document
                    {
                        CreatedByUserId = createModel.CreatedByUserId,
                        CreatedOnDate = DateTime.Now,
                        DocumentId = 1,
                        FileCates = createModel.FileCates,
                        FileUrl = createModel.FileUrl,
                        LastModifiedByUserId = createModel.CreatedByUserId,
                        LastModifiedOnDate = DateTime.Now,
                        Name = createModel.Name,
                        Description = createModel.Description,
                        DirectoryId = createModel.DirectoryId,
                        DocumentType = createModel.DocumentType,
                        IsDelete = false,
                        DepartmentId= user.DepartmentId
                    };


                    if (last != null) doc.DocumentId = last.DocumentId + 1;
                    unitOfWork.GetRepository<Document>().Add(doc);
                    if (unitOfWork.Save() >= 1)
                    {
                        return GetById(doc.DocumentId);
                    }
                    return new Response<DocumentModel>(0, "Lưu thông tin không thành công", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentModel> Delete(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var doc = unitOfWork.GetRepository<Document>().GetById(Id);
                    if (doc != null)
                    {
                        doc.IsDelete = true;
                        unitOfWork.GetRepository<Document>().Update(doc);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(doc.DocumentId);
                        }
                        return new Response<DocumentModel>(0, "Lưu thông tin không thành công!", null);
                    }
                    else
                        return new Response<DocumentModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentModel>(-1, ex.ToString(), null);
            }
        }
        public Response<List<DocumentDisplayModel>> GetAll()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<Document>().GetMany(u => u.IsDelete == false)
                        .Join(unitOfWork.GetRepository<User>().GetAll(),
                        d => d.CreatedByUserId,
                        u => u.UserId,
                        (d, u) => new DocumentDisplayModel
                        {
                            CreatedByUserId = d.CreatedByUserId,
                            CreatedOnDate = d.CreatedOnDate,
                            DocumentId = d.DocumentId,
                            FileCates = d.FileCates,
                            FileUrl = d.FileUrl,
                            LastModifiedByUserId = d.LastModifiedByUserId,
                            LastModifiedOnDate = d.LastModifiedOnDate,
                            Name = d.Name,
                            CreatedByUserName = u.UserName
                        })
                        .OrderByDescending(u => u.DocumentId)
                        .ToList();
                    return new Response<List<DocumentDisplayModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentDisplayModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<List<DocumentDisplayModel>> GetAll(int UserId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user == null) return new Response<List<DocumentDisplayModel>>(0, "", null);

                    var list = unitOfWork.GetRepository<Document>().GetMany(u => u.IsDelete == false);
                    if (user.UserRoleId > 1)
                    {
                       // var listDir = unitOfWork.GetRepository<Directory>().GetMany(d => d.DepartmentId == user.DepartmentId).Select(d => d.DirectoryId).ToList();

                        list = list.Where(u => u.DepartmentId == user.DepartmentId);
                    }
                    var result = list.Join(unitOfWork.GetRepository<User>().GetAll(),
                        d => d.CreatedByUserId,
                        u => u.UserId,
                        (d, u) => new DocumentDisplayModel
                        {
                            CreatedByUserId = d.CreatedByUserId,
                            CreatedOnDate = d.CreatedOnDate,
                            DocumentId = d.DocumentId,
                            FileCates = d.FileCates,
                            FileUrl = d.FileUrl,
                            LastModifiedByUserId = d.LastModifiedByUserId,
                            LastModifiedOnDate = d.LastModifiedOnDate,
                            Name = d.Name,
                            CreatedByUserName = u.UserName
                        })
                        .OrderByDescending(u => u.DocumentId)
                        .ToList();
                    return new Response<List<DocumentDisplayModel>>(1, "", result);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentDisplayModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<List<DocumentModel>> GetByType(int TypeId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<Document>().GetMany(u => u.IsDelete == false)
                        .AsEnumerable().Where(d => IsInCate(TypeId, d.FileCates))
                         .Select(u => new DocumentModel
                         {
                             CreatedByUserId = u.CreatedByUserId,
                             CreatedOnDate = u.CreatedOnDate,
                             DocumentId = u.DocumentId,
                             FileCates = u.FileCates,
                             FileUrl = u.FileUrl,
                             LastModifiedByUserId = u.LastModifiedByUserId,
                             LastModifiedOnDate = u.LastModifiedOnDate,
                             Name = u.Name,
                             Description = u.Description,
                             DirectoryId = u.DirectoryId
                         })
                         .OrderByDescending(u => u.DocumentId)
                         .ToList();
                    return new Response<List<DocumentModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<List<DocumentModel>> GetByUser(int UserId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<Document>().GetMany(u => u.IsDelete == false && u.CreatedByUserId == UserId)
                         .Select(u => new DocumentModel
                         {
                             CreatedByUserId = u.CreatedByUserId,
                             CreatedOnDate = u.CreatedOnDate,
                             DocumentId = u.DocumentId,
                             FileCates = u.FileCates,
                             FileUrl = u.FileUrl,
                             LastModifiedByUserId = u.LastModifiedByUserId,
                             LastModifiedOnDate = u.LastModifiedOnDate,
                             Name = u.Name,
                             Description = u.Description,
                             DirectoryId = u.DirectoryId

                         })
                         .OrderByDescending(u => u.DocumentId)
                         .ToList();
                    return new Response<List<DocumentModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentModel>>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentModel> GetById(int Id)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var doc = unitOfWork.GetRepository<Document>().GetById(Id);
                    if (doc != null) return new Response<DocumentModel>(1, "", Ultis.ConvertSameData<DocumentModel>(doc));
                    else
                        return new Response<DocumentModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentModel>(-1, ex.ToString(), null);
            }
        }
        public Response<DocumentModel> Update(int Id, DocumentUpdateModel updateModel)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var doc = unitOfWork.GetRepository<Document>().GetById(Id);
                    if (doc != null)
                    {
                        Ultis.TransferValues(doc, updateModel);
                        unitOfWork.GetRepository<Document>().Update(doc);
                        if (unitOfWork.Save() >= 1)
                        {
                            return GetById(doc.DocumentId);
                        }
                        return new Response<DocumentModel>(0, "Lưu thông tin không thành công!", null);
                    }
                    else
                        return new Response<DocumentModel>(0, "Không tìm thấy tài liệu", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentModel>(-1, ex.ToString(), null);
            }
        }
        public Response<List<DocumentDisplayModel>> GetDocsInDirectory(int DirectoryId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<Document>().GetMany(u => u.IsDelete == false && u.DirectoryId == DirectoryId)
                          .Join(unitOfWork.GetRepository<User>().GetAll(),
                        d => d.CreatedByUserId,
                        u => u.UserId,
                        (d, u) => new DocumentDisplayModel
                        {
                            CreatedByUserId = d.CreatedByUserId,
                            CreatedOnDate = d.CreatedOnDate,
                            DocumentId = d.DocumentId,
                            FileCates = d.FileCates,
                            FileUrl = d.FileUrl,
                            LastModifiedByUserId = d.LastModifiedByUserId,
                            LastModifiedOnDate = d.LastModifiedOnDate,
                            Name = d.Name,
                            CreatedByUserName = u.UserName,

                        })
                        .OrderByDescending(u => u.DocumentId)
                        .ToList();
                    return new Response<List<DocumentDisplayModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentDisplayModel>>(-1, ex.ToString(), null);
            }
        }
        private bool IsInCate(int cateid, string list)
        {
            return list.Split(',').Contains(cateid.ToString());
        }

        public Response<List<DocumentDisplayModel>> GetAll(FilterDocument filter)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {

                    var list = unitOfWork.GetRepository<Document>().GetMany(u => u.IsDelete == false && !u.DocumentType);
                    if (filter.DepartmentId != null)
                    {
                        var _listDirs = unitOfWork.GetRepository<Directory>().GetMany(d => d.DepartmentId == filter.DepartmentId).Select(d => d.DirectoryId).ToList();
                        if (filter.DirectoryId != null)
                        {
                            list = list.Where(d => d.DirectoryId == filter.DirectoryId);
                        }
                        else
                        {
                            list = list.Where(d => _listDirs.Contains(d.DirectoryId));
                        }
                    }
                    if (filter.CategoryId != null)
                    {
                        list = list.Where(d => IsInCate(Convert.ToInt32(filter.CategoryId), d.FileCates));
                    }

                    if (filter.StartDate != null)
                    {
                        list = list.Where(d => d.CreatedOnDate >= filter.StartDate.Value);
                    }
                    if (filter.EndDate != null)
                    {
                        list = list.Where(d => d.CreatedOnDate <= filter.EndDate.Value);
                    }
                    if (filter.UserId != null)
                    {
                        list = list.Where(d => d.CreatedByUserId == filter.UserId);
                    }

                    if (filter.Name.Length > 0)
                    {
                        list = list.Where(d => d.Name.Trim().ToLower().Contains(filter.Name.Trim().ToLower()));
                    }
                    var result = list.Join(unitOfWork.GetRepository<User>().GetAll(),
                        d => d.CreatedByUserId,
                        u => u.UserId,
                        (d, u) => new DocumentDisplayModel
                        {
                            CreatedByUserId = d.CreatedByUserId,
                            CreatedOnDate = d.CreatedOnDate,
                            DocumentId = d.DocumentId,
                            FileCates = d.FileCates,
                            FileUrl = d.FileUrl,
                            LastModifiedByUserId = d.LastModifiedByUserId,
                            LastModifiedOnDate = d.LastModifiedOnDate,
                            Name = d.Name,
                            CreatedByUserName = u.UserName
                        })
                        .OrderByDescending(u => u.DocumentId)
                        .ToList();
                    return new Response<List<DocumentDisplayModel>>(1, "", result);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentDisplayModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<DocumentModel> ShareToDepartment(int UserId, ShareDocumentModel model)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    if (model.DepartmentId == 0) return new Response<DocumentModel>(0, "Bạn không thể gửi cho phòng văn thư", null);
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user == null) return new Response<DocumentModel>(0, "User doesn't exist!", null);
                    if (user.DepartmentId != 0) return new Response<DocumentModel>(0, "Chỉ phòng văn thư mới gửi được", null);
                    var exist = unitOfWork.GetRepository<QH_ShareDocument_Department>().Get(d => d.DepartmentId == model.DepartmentId && d.DocumentId == model.DocumentId);
                    if (exist != null) return new Response<DocumentModel>(0, "Tài liệu đã được gửi cho phòng ban này", null);
                    QH_ShareDocument_Department qh = new QH_ShareDocument_Department
                    {
                        CreatedByUserId = UserId,
                        CreatedOnDate = DateTime.Now,
                        DepartmentId = model.DepartmentId,
                        DocumentId = model.DocumentId,
                        Message = model.Message
                    };
                    unitOfWork.GetRepository<QH_ShareDocument_Department>().Add(qh);
                    if (unitOfWork.Save() >= 1)
                    {
                        return new Response<DocumentModel>(1, "", null);
                    }
                    return new Response<DocumentModel>(0, "", null);
                }
            }
            catch (Exception ex)
            {
                return new Response<DocumentModel>(-1, ex.ToString(), null);
            }
        }
        public Response<List<DocumentDisplayModel>> GetShareDocuments(int UserId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var user = unitOfWork.GetRepository<User>().GetById(UserId);
                    if (user == null) return new Response<List<DocumentDisplayModel>>(0, "", null);

                    var list = unitOfWork.GetRepository<Document>().GetMany(u => u.IsDelete == false)
                        .Join(unitOfWork.GetRepository<QH_ShareDocument_Department>().GetMany(d => d.DepartmentId == user.DepartmentId),
                        d => d.DocumentId,
                        qh => qh.DocumentId,
                        (d, qh) => new
                        {
                            CreatedByUserId = d.CreatedByUserId,
                            CreatedOnDate = d.CreatedOnDate,
                            DocumentId = d.DocumentId,
                            FileCates = d.FileCates,
                            FileUrl = d.FileUrl,
                            LastModifiedByUserId = d.LastModifiedByUserId,
                            LastModifiedOnDate = d.LastModifiedOnDate,
                            Name = d.Name,
                            DirectoryId = d.DirectoryId
                        }
                        );

                    var result = list.Join(unitOfWork.GetRepository<User>().GetAll(),
                        d => d.CreatedByUserId,
                        u => u.UserId,
                        (d, u) => new DocumentDisplayModel
                        {
                            CreatedByUserId = d.CreatedByUserId,
                            CreatedOnDate = d.CreatedOnDate,
                            DocumentId = d.DocumentId,
                            FileCates = d.FileCates,
                            FileUrl = d.FileUrl,
                            LastModifiedByUserId = d.LastModifiedByUserId,
                            LastModifiedOnDate = d.LastModifiedOnDate,
                            Name = d.Name,
                            CreatedByUserName = u.UserName
                        })
                        .OrderByDescending(u => u.DocumentId)
                        .ToList();
                    return new Response<List<DocumentDisplayModel>>(1, "", result);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentDisplayModel>>(-1, ex.ToString(), null);
            }
        }

        public Response<List<DocumentDisplayModel>> GetAllInDepartment(int DepartmentId)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    var list = unitOfWork.GetRepository<Document>().GetMany(u => u.IsDelete == false && u.DepartmentId == DepartmentId)
                          .Join(unitOfWork.GetRepository<User>().GetAll(),
                        d => d.CreatedByUserId,
                        u => u.UserId,
                        (d, u) => new DocumentDisplayModel
                        {
                            CreatedByUserId = d.CreatedByUserId,
                            CreatedOnDate = d.CreatedOnDate,
                            DocumentId = d.DocumentId,
                            FileCates = d.FileCates,
                            FileUrl = d.FileUrl,
                            LastModifiedByUserId = d.LastModifiedByUserId,
                            LastModifiedOnDate = d.LastModifiedOnDate,
                            Name = d.Name,
                            CreatedByUserName = u.UserName,

                        })
                        .OrderByDescending(u => u.DocumentId)
                        .ToList();
                    return new Response<List<DocumentDisplayModel>>(1, "", list);
                }
            }
            catch (Exception ex)
            {
                return new Response<List<DocumentDisplayModel>>(-1, ex.ToString(), null);
            }
        }
    }
}