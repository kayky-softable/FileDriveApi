using System.Runtime.InteropServices.JavaScript;
using Amazon.S3;
using Amazon.S3.Model;
using FileDriveAPi.Data;
using FileDriveAPi.Model.Entity;
using Microsoft.AspNetCore.Mvc;

namespace FileDriveAPi.Services;

public class FileDriveService 
{ 
    public async Task DeleteFile(Guid id ,AmazonS3Client s3Client,FileDriveApiDbContext fileDriveApiDbContext)
    {
        var deleteRequest = new DeleteObjectRequest()
        {
            BucketName = "file-drive-api-bucket",
            Key = id.ToString()
        };
        
        var file = await fileDriveApiDbContext.MyFiles.FindAsync(id);
        if (file != null)fileDriveApiDbContext.MyFiles.Remove(file);
            
        await fileDriveApiDbContext.SaveChangesAsync(); 
        await s3Client.DeleteObjectAsync(deleteRequest);
        
        
        
    }

    public async Task UploadFile(IFormFile file,AmazonS3Client s3Client,FileDriveApiDbContext fileDriveApiDbContext)
    {
        var id = Guid.NewGuid();
        MyFile myFile = new MyFile()
        {
            Id = id,
            UploadDateTime = DateTime.UtcNow,
            FileName = file.FileName,
            
        };
        Console.WriteLine(myFile.UploadDateTime);
        
        byte[] fileBytes;
        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            fileBytes = ms.ToArray();
            
        }

        var request = new PutObjectRequest()
        {
            ContentType = file.ContentType,
            InputStream = new MemoryStream(fileBytes),
            BucketName = "file-drive-api-bucket",
            Key = id.ToString(),
        };
        
        fileDriveApiDbContext.Add(myFile);
        await s3Client.PutObjectAsync(request);
        await fileDriveApiDbContext.SaveChangesAsync();
        
    }

   
    public async Task<(byte[] FileBytes, string ContentType)> GetFileByIdAsync(Guid id, AmazonS3Client s3Client,FileDriveApiDbContext fileDriveApiDbContext)
    {
        // Criando a solicitação GetObject
        var request = new GetObjectRequest
        {
            BucketName = "file-drive-api-bucket",
            Key = id.ToString(),
        };

        try
        {
            // Executando a solicitação e obtendo a resposta
            var response = await s3Client.GetObjectAsync(request);

            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await response.ResponseStream.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            // Retorna o arquivo e o Content-Type
            return (fileBytes, response.Headers.ContentType);
        }
        catch (AmazonS3Exception ex)
        {
            // Em caso de erro no S3, lançamos a exceção novamente ou podemos fazer um tratamento específico
            throw new Exception($"Error retrieving file: {ex.Message}");
        }
    }

    public List<MyFile> GetAllFiles(FileDriveApiDbContext fileDriveApiDbContext)
    {
        return  fileDriveApiDbContext.MyFiles.ToList();
    }
    
}

    
