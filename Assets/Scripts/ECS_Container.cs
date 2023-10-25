using System;
using Unity.Jobs;
using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Rendering;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Rendering;
using Unity.Transforms;

[MaterialProperty("_Color")]
public struct MaterialColor : IComponentData
{
	public float4 Value;
}

[MaterialProperty("_BaseColor")]
public struct MaterialBaseColor : IComponentData
{
	public float4 Value;
}

[MaterialProperty("_SpecColor")]
public struct MaterialSpecColor : IComponentData
{
	public float4 Value;
}

[MaterialProperty("_EmissionColor")]
public struct MaterialEmissionColor : IComponentData
{
	public float4 Value;
}

public class ECS_Container
{
	public static Entity Create(EntityManager entityManager, Mesh mesh, Material mat)
	{
		var entity = entityManager.CreateEntity();
		RenderMeshUtility.AddComponents(
			entity,
			entityManager,
			new RenderMeshDescription(ShadowCastingMode.Off, false),
			new RenderMeshArray(new Material[] { mat }, new Mesh[] { mesh }),
			MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0)
		);
		entityManager.AddComponent(entity, typeof(LocalToWorld));
		return entity;
	}

	public static Entity Color(EntityManager entityManager, Entity entity, float3 color)
	{
		return entity;
	}

	// Create a BRG object and allocate buffers. 
	public static NativeArray<Entity> CreateEntitys(Mesh mesh, Material mat, int instanceSize, bool castShadows)
    {
		NativeArray<Entity> entitys = new NativeArray<Entity>(instanceSize, Allocator.Temp);
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var prototype = ECS_Container.Create(entityManager, mesh, mat);

		for (int i = 0; i < instanceSize; i++)
        {
			var entity = entityManager.Instantiate(prototype);
			entitys[i] = entity;
		}

		entityManager.DestroyEntity(prototype);
		return entitys;
    }

	public static EntityQuery CreateEntityQuery(params ComponentType[] requiredComponents)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		return entityManager.CreateEntityQuery(requiredComponents);
	}
}
