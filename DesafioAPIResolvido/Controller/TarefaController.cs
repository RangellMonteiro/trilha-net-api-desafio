using Microsoft.AspNetCore.Mvc;
using DesafioAPIResolvido.Context;
using DesafioAPIResolvido.Models;

namespace DesafioAPIResolvido.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var tarefa = _context.Tarefas.Find(id); // Busca a tarefa pelo ID

            if (tarefa == null) // Verifica se a tarefa foi encontrada
            {
                return NotFound(); // Retorna 404 se não encontrar
            }

            return Ok(tarefa); // Retorna 200 com a tarefa encontrada
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefas = _context.Tarefas.ToList(); // Busca todas as tarefas no banco
            return Ok(tarefas); // Retorna 200 com a lista de tarefas
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefas = _context.Tarefas.Where(t => t.Titulo.Contains(titulo)).ToList(); // Busca tarefas pelo título
            return Ok(tarefas); // Retorna 200 com as tarefas encontradas
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date).ToList(); // Busca tarefas pela data
            return Ok(tarefa); // Retorna 200 com as tarefas encontradas
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefas = _context.Tarefas.Where(x => x.Status == status).ToList(); // Busca tarefas pelo status
            return Ok(tarefas); // Retorna 200 com as tarefas encontradas
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Tarefas.Add(tarefa); // Adiciona a tarefa ao contexto
            _context.SaveChanges(); // Salva as mudanças no banco

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa); // Retorna 201 com a tarefa criada
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id); // Busca a tarefa no banco

            if (tarefaBanco == null)
                return NotFound(); // Retorna 404 se não encontrar

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo; // Atualiza o título
            tarefaBanco.Descricao = tarefa.Descricao; // Atualiza a descrição
            tarefaBanco.Data = tarefa.Data; // Atualiza a data
            tarefaBanco.Status = tarefa.Status; // Atualiza o status

            _context.SaveChanges(); // Salva as mudanças

            return Ok(tarefaBanco); // Retorna 200 com a tarefa atualizada
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id); // Busca a tarefa no banco

            if (tarefaBanco == null)
                return NotFound(); // Retorna 404 se não encontrar

            _context.Tarefas.Remove(tarefaBanco); // Remove a tarefa do contexto
            _context.SaveChanges(); // Salva as mudanças

            return NoContent(); // Retorna 204 sem conteúdo
        }
    }
}
