﻿module PirateUpdate

open GameState
open MapObject
open CellLocation
open Random
open NPCEncounterUtilities
open ActorUtilities


let updatePirateActor 
    (sumLocationsFunc:SumLocationsFunc) 
    (random:RandomFunc) 
    (actorLocation:CellLocation) 
    (actorProperties:PirateProperties) 
    (actorTurn:float<turn>) 
    (currentTurn:float<turn>) 
    (playState:PlayState<_>) :PlayState<_> =
    if currentTurn <= actorTurn then
        playState
    else
        let actors' = 
            playState.Actors 
            |> Map.remove actorLocation

        let actorLocation' = 
            actorLocation 
            |> sumLocationsFunc {Column=((-1,2) |> randomIntRange random ) * 1<cell>;Row=((-1,2) |> randomIntRange random ) * 1<cell>}

        let actorTurn' = 
            actorTurn + 1.0<turn>

        let actor' = 
            {CurrentTurn = actorTurn';
             Detail      = actorProperties |> Pirate}

        let otherActor = actors'.TryFind actorLocation'
        if otherActor.IsNone then
            playState
            |> placeActor actors' actorLocation' actor'
        else
            match otherActor.Value.Detail with
            | Boat properties       -> {playState with Actors = actors' |> Map.add actorLocation actor'}
            | Storm properties      -> {playState with Actors = actors' |> Map.add actorLocation actor'}
            | Pirate properties     -> {playState with Actors = actors' |> Map.add actorLocation actor'}
            | SeaMonster properties -> {playState with Actors = actors' |> Map.add actorLocation actor'}
            | Merfolk properties    -> {playState with Actors = actors' |> Map.add actorLocation actor'}
            | _                     -> {playState with Actors = actors' |> Map.add actorLocation actor'}


