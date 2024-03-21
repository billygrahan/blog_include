using bog_include.Context;
using bog_include.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;



namespace blog_include.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostagemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostagemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postagem>>> Get()
        {
            var postagens = await _context.Postagems.AsNoTracking().ToListAsync();
            if (postagens.Count == 0) // Verifica se não há postagens
            {
                return NotFound("Não há postagens");
            }
            return postagens;
        }

        [HttpGet("{id:int:min(1)}", Name = "Obterpostagem")]
        public ActionResult<Postagem> Get(int id)
        {
            var postagem = _context.Postagems.FirstOrDefault(p => p.PostagemId == id);
            if (postagem is null)
            {
                return NotFound("postagem não encontrada");
            }

            return postagem;
        }

        [HttpGet("{id:int:min(1)}/imagem", Name = "ObterImagemPostagem")]
        public IActionResult GetImagem(int id)
        {
            var postagem = _context.Postagems.FirstOrDefault(p => p.PostagemId == id);
            if (postagem == null || postagem.Imagem == null)
            {
                return NotFound("Imagem não encontrada");
            }

            // Converte a imagem binária para um array de bytes
            byte[] imagemBytes = postagem.Imagem;

            // Retorna a imagem como um arquivo para a resposta HTTP
            return File(imagemBytes, "image/jpeg"); // Altere o tipo de conteúdo conforme necessário
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Postagem postagem)
        {
            var produtoExistente = _context.Postagems.FirstOrDefault(p => p.PostagemId == id);
            if (produtoExistente == null)
            {
                return NotFound("Postagem não encontrada");
            }

            // Atualiza apenas os campos específicos da postagem
            produtoExistente.Titulo = postagem.Titulo;
            produtoExistente.Mensagem = postagem.Mensagem;
            // Atualize outros campos conforme necessário

            _context.SaveChanges();

            return Ok(produtoExistente);
        }

        [HttpPut("carregaimagem/{id:int}")]
        public async Task<IActionResult> UploadImagem(int id, IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                return BadRequest("Arquivo não enviado.");
            }

            var postagem = await _context.Postagems.FindAsync(id);
            if (postagem == null)
            {
                return NotFound("Postagem não encontrada.");
            }

            using (var memoryStream = new System.IO.MemoryStream())
            {
                await arquivo.CopyToAsync(memoryStream);

                // Se a imagem for maior que um determinado tamanho, você pode querer rejeitá-la ou redimensioná-la aqui
                postagem.Imagem = memoryStream.ToArray();
            }

            _context.Entry(postagem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostagemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Ou retorne um status 200 OK com alguma informação se preferir
        }

        private bool PostagemExists(int id)
        {
            return _context.Postagems.Any(e => e.PostagemId == id);
        }


    [HttpPost]
        public ActionResult Post(Postagem postagem)
        {
            if (postagem is null)
            {
                return BadRequest("erro ao carregar o produto!");
            }
            _context.Postagems.Add(postagem);
            _context.SaveChanges();

            return new CreatedAtRouteResult("Obterpostagem", new { id = postagem.PostagemId }, postagem);
        }


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var postagem = _context.Postagems.FirstOrDefault(p => p.PostagemId == id);
            if (postagem == null)
            {
                return NotFound("Postagem não encontrada");
            }
            _context.Postagems.Remove(postagem);
            _context.SaveChanges();

            return Ok(postagem);
        }

    }
}

