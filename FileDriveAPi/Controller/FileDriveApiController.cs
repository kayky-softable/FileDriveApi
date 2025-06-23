using Amazon.S3;
using FileDriveAPi.Data;
using FileDriveAPi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileDriveAPi.Controller;

public class FileDriveApiController : Microsoft.AspNetCore.Mvc.Controller
{
    FileDriveService _fileDriveService;
    private AmazonS3Client _s3Client;
    private FileDriveApiDbContext _fileDriveApiDbContext;

    public FileDriveApiController(AmazonS3Client s3Client, FileDriveApiDbContext fileDriveApiDbContext, FileDriveService fileDriveService)
    {
        this._fileDriveService= fileDriveService;
        _s3Client = s3Client;
        _fileDriveApiDbContext = fileDriveApiDbContext;
    }
    [HttpPost]
    [Route("/file/[controller]")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        try
        {
            await _fileDriveService.UploadFile(file, _s3Client, _fileDriveApiDbContext);
            return Ok($"arquivo {file.FileName} salvo com sucesso");
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpDelete]
    [Route("/file/{id}")]
    //Delete
    public async Task<IActionResult> DeleteById(Guid id)
    {
        try
        { 
            await _fileDriveService.DeleteFile(id, _s3Client, _fileDriveApiDbContext);
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("/file/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var (fileBytes, contentType) = await _fileDriveService.GetFileByIdAsync(id, _s3Client, _fileDriveApiDbContext);

            // Configura os cabeçalhos de resposta para download
            Response.Headers.Append("Content-Disposition", $"attachment; filename={id.ToString()}");

            // Retorna o arquivo com o tipo de conteúdo adequado
            return File(fileBytes, contentType, id.ToString());
        }catch(Exception e){
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("/file")]
    public IActionResult GetAllFiles()
    {
        try
        {
            return Ok(_fileDriveService.GetAllFiles(_fileDriveApiDbContext));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}