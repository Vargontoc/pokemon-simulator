// See https://aka.ms/new-console-template for more information
using poke.battle.core;
using poke.battle.factories;
using poke.battle.services;
using poke.battle.services.impl;
using poke_battle_console;
using poke_battle_services.factories;

Console.WriteLine("\n=== Simulación de batalla pokemon ===\n");


// Servicios
var typesService = new TypesService(new HttpClient());
TypeEffectivenessResolver.Initialize(typesService.GetAll(null!, null!).Results);

IMoveService moveService = new MovesService(new HttpClient());
IPokemonService pokemonService = new PokemonsService(new HttpClient());

// Factory
var factory = new BattlerFactory(pokemonService, moveService);
BurnEffectTest.Run(factory);


Console.WriteLine("\n=== Fin simulación ===\n");



