using Microsoft.AspNetCore.Mvc;
using poke.battle.api.utils;
using poke.battle.infraestructure.filters.impl;
using poke.battle.infraestructure.repositories;
using poke.battle.Models.Impl;
using poke.battle.services;
using poke_battle_api.controllers;
using poke_battle_api.services;
using poke_battle_infraestructure.validators;

namespace poke.battle.api.Controllers 
{ 
    [ApiController]
    [Route("api/ability")]
    public class AbilitiesController(IAbilityService abilities, IGeneratorAIService generator) : Controller
    {
        [HttpGet("{id}")]
        public ActionResult<AbilityModel> GetAbility(int id)
        {
            var result = abilities.GetById(id);
            return Ok(result);
        }

        [HttpPost("query")]
        public IActionResult GetAbilities([FromBody] QueryRequest<AbilitiesFilter> query)
        {
            var result = abilities.FindAll(query.Filter, query.Request);
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<AbilityModel> Save([FromBody] AbilityModel entity)
        {
            try { 
                var saved = abilities.Save(entity);
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
        public ActionResult<AbilityModel> Update([FromBody] AbilityModel entity)
        {
            try
            {
                var saved = abilities.Update(entity);
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

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var deleted = abilities.Delete(id);
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

        [HttpGet("generate")]
        public async Task<ActionResult> Generate()
        {
            var botResponse = await generator.GenerateAbility();
            return Ok(botResponse);
        }
    }
}