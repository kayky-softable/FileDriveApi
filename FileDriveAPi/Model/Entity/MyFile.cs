using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace FileDriveAPi.Model.Entity
{
    public class MyFile
    {
        public Guid Id { get; set; }  // Coluna da tabela

        [Required] // Coluna obrigatória no banco
        public string FileName { get; set; }  

        [Required] // Coluna obrigatória no banco
        public DateTime UploadDateTime { get; set; } = DateTime.UtcNow; // Default para data e hora atual

        [NotMapped] // Não mapeado para o banco de dados
        public string ContentType { get; set; }  

        [NotMapped]
        public string ContentDisposition { get; set; } 

        [NotMapped]
        public IHeaderDictionary Headers { get; set; } = null!;

        [NotMapped]
        public long Length { get; set; }  

        [NotMapped]
        public string Name { get; set; } = null!;

        // Implementação de IFormFile: os métodos serão deixados não implementados
        // Já que você não deseja realmente armazenar ou manipular esses dados aqui
        public Stream OpenReadStream()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Stream target)
        {
            throw new NotImplementedException();
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}