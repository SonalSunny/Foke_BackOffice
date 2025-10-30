using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.FileUpload.DTO;
using FOKE.Entity.FileUpload.ViewModel;
using FOKE.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FOKE.Services.Repository
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly FOKEDBContext _context;
        public AttachmentRepository(FOKEDBContext context)
        {
            _context = context;
        }


        public async Task<ResponseEntity<FileStorage>> SaveAttachment(FileStorageViewModel objModel)
        {
            var response = new ResponseEntity<FileStorage>();
            try
            {
                string fileName = Guid.NewGuid().ToString();
                string relativePath;
                string fullFilePath;

                if (!Directory.Exists(objModel.FilePath))
                {
                    Directory.CreateDirectory(objModel.FilePath);
                }

                fullFilePath = Path.Combine(objModel.FilePath, fileName + objModel.FileExtension);
                File.WriteAllBytes(fullFilePath, objModel.FileData);

                var normalizedPath = fullFilePath.Replace("\\", "/");
                var folderIndex = normalizedPath.IndexOf($"/FileStorage", StringComparison.OrdinalIgnoreCase);
                relativePath = folderIndex >= 0 ? normalizedPath.Substring(folderIndex) : normalizedPath;

                var fileStorage = new FileStorage
                {
                    FileName = fileName,
                    OrgFileName = objModel.FileName,
                    FileExtension = objModel.FileExtension,
                    ContentType = objModel.ContentType,
                    ContentLength = objModel.ContentLength,
                    FilePath = relativePath,
                    StorageMode = normalizedPath,
                    Active = true,
                    CreatedBy = objModel.CreatedBy,
                    CreatedDate = DateTime.Now,

                };

                await _context.FileStorages.AddAsync(fileStorage);
                await _context.SaveChangesAsync();

                response.returnData = fileStorage;
                response.transactionStatus = HttpStatusCode.OK;
            }
            catch (Exception)
            {
                response.transactionStatus = HttpStatusCode.InternalServerError;
                response.returnMessage = "Internal Server Error Occurred";
            }

            return response;
        }
        public ResponseEntity<bool> DeleteAttachment(long Id)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var FileData = _context.FileStorages.FirstOrDefault(w => w.FileStorageId == Id);
                if (FileData != null)
                {
                    FileData.Active = false;
                    _context.Entry(FileData).State = EntityState.Modified;
                    _context.SaveChanges();
                    retModel.returnData = true;
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }
        public ResponseEntity<List<AttachmentViewModel>> GetAttachmentById(string encryptedID)
        {
            var retData = new ResponseEntity<List<AttachmentViewModel>>();
            try
            {
                var objResponseList = new List<AttachmentViewModel>();
                var objResponse = new AttachmentViewModel();
                var masterId = GenericUtilities.Convert<long>(encryptedID);

                var objAttachment = _context.Attachments
                    .Where(c => c.AttachmentMasterId == masterId && c.Active)
                    .Include(c => c.FileStorage)
                    .ToList();

                if (objAttachment.Any())
                {
                    foreach (var attachment in objAttachment)
                    {
                        objResponse.AttachmentId = attachment.AttachmentId;
                        objResponse.FileName = attachment.FileName;
                        objResponse.ContentType = attachment.FileStorage.ContentType;
                        objResponse.ContentLength = attachment.FileStorage.ContentLength;
                        objResponse.FileExtension = attachment.FileStorage.FileExtension;
                        objResponse.FileStorageId = attachment.FileStorage.FileStorageId;
                        objResponse.ActualFileName = attachment.OrgFileName;

                        var filepath = attachment.FileStorage.FilePath;

                        var storageRoot = Path.Combine(Directory.GetCurrentDirectory(), "FileStorage");
                        var relativePath = filepath.Replace(storageRoot, "").TrimStart('\\', '/');

                        relativePath = relativePath.Replace("\\", "/");

                        objResponse.FilePath = $"/report/downloadFile/{relativePath}";

                        using (var memory = new MemoryStream())
                        {
                            try
                            {
                                using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                                {
                                    stream.CopyTo(memory);
                                }

                                memory.Position = 0;
                                objResponse.RetFileData = Convert.ToBase64String(memory.ToArray());
                            }
                            catch (Exception ex)
                            {
                                objResponse.Message = $"An error occurred while processing the file. Details: {ex.Message}. StackTrace: {ex.StackTrace}";
                            }
                        }
                        objResponseList.Add(objResponse);
                    }
                    retData.returnData = objResponseList;

                }
            }
            catch (Exception ex)
            {
                retData.returnMessage = $"An error occurred while processing the file. Details: {ex.Message}. StackTrace: {ex.StackTrace}";
            }

            retData.transactionStatus = System.Net.HttpStatusCode.OK;
            return retData;
        }
    }
}
