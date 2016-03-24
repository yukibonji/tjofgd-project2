﻿module SeaMonsterUpdate

open GameState
open MapObject
open CellLocation
open Random
open NPCEncounterUtilities

let updateSeaMonsterActor 
    (sumLocationsFunc:SumLocationsFunc) 
    (random:RandomFunc) 
    (actorLocation:CellLocation) 
    (actorProperties:SeaMonsterProperties) 
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
            actorTurn + 2.0<turn>

        let actor' = 
            {CurrentTurn = actorTurn';
                Detail      = actorProperties |> SeaMonster}

        let otherActor = actors'.TryFind actorLocation'
        if otherActor.IsNone then

            let actors'' =
                actors' 
                |> Map.add actorLocation' actor'

            {playState with Actors = actors''}
        else
            match otherActor.Value.Detail with
            | _                 -> {playState with Actors = actors' |> Map.add actorLocation actor'}


