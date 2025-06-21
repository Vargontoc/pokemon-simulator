// See https://aka.ms/new-console-template for more information
using poke.battle.core;
using poke.battle.factories;
using poke.battle.infraestructure.repositories.impl;
using poke.battle.services;
using poke.battle.services.impl;
using poke_battle_console;
using poke_battle_services.factories;
using System.Linq.Expressions;

Console.WriteLine("\n=== Simulación de batalla pokemon ===\n");


// Servicios
var typesService = new TypesService(new TypesRepository(), null!, null!, null!);
TypeEffectivenessResolver.Initialize(typesService.FindAll(null!, null!).Results);

IMoveService moveService = new MovesService(new MovesRepository());
IPokemonService pokemonService = new PokemonsService(new PokemonsRepository());

// Factory
var factory = new BattlerFactory(pokemonService, moveService);
BurnEffectTest.Run(factory);


Console.WriteLine("\n=== Fin simulación ===\n");



