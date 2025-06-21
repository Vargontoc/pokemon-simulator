using Microsoft.AspNetCore.Mvc;
using poke.battle.api.utils;
using poke.battle.infraestructure.filters;
using poke.battle.infraestructure.repositories;
using poke.battle.Models.Impl;
using poke.battle.services;
using poke_battle_api.dtos;
using poke_battle_api.mappers;
using poke_battle_infraestructure.validators;
namespace poke_battle_api.controllers
{
    [ApiController]
    [Route("api/types")]
    public class TypesController(ITypeService service, IMapper<TypeDto, TypeModel> mapper) : Controller
    {

        [HttpGet("{id}")]
        public IActionResult GetType(int id)
        {
            var result = service.GetById(id);
            return Ok(mapper.convertToFront(result));
        }

        [HttpPost("query")]
        public IActionResult GetTypes([FromBody] QueryRequest<TypesFilter> query)
        {
            var result = service.FindAll(query.Filter, query.Request);
            var list = mapper.convertToFront(result.Results);


            return Ok(new PageResponse<TypeDto>()
            {
                Page = result.Page,
                PageSize = result.PageSize,
                Total = result.Total,
                Results = list.ToList()
            });
        }

        [HttpPost]
        public IActionResult Save([FromBody] TypeDto model)
        {
            try
            {
                var saved = service.Save(mapper.convertToBack(model));
                return Ok(saved);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new { message = ex.Message, errors = new[] { ex.InnerException } });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message, errors = ex.Errors });
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] TypeDto model)
        {
            try
            {
                var saved = service.Update(mapper.convertToBack(model));
                return Ok(saved);
            }
            catch(RepositoryException ex)
            {
                return BadRequest(new { message = ex.Message, errors = new [] { ex.InnerException } });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message, errors = ex.Errors });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var deleted = service.Delete(id);
                return deleted ? Ok(deleted) : BadRequest(new { message = "Error", errors = new[] { "No se pudo eliminar la habilidad" } });
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new { message = ex.Message, errors = new[] { ex.InnerException } });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message, errors = ex.Errors });
            }
        }
    }
}
