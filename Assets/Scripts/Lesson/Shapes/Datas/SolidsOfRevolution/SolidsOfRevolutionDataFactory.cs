// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.Serialization;
// using JetBrains.Annotations;
// using Lesson.Shapes.Datas.ShapesOfRevolution;
// using Lesson.Shapes.Views;
// using Lesson.Validators.Uniqueness;
// using Newtonsoft.Json;
// using UnityEngine;
//
// namespace Lesson.Shapes.Datas.SolidsOfRevolution
// {
//     [JsonObject(IsReference = true, MemberSerialization = MemberSerialization.OptIn)]
//     public class SolidsOfRevolutionDataFactory
//     {
//         public event Action BecameDirty;
//         
//         [JsonProperty]
//         private readonly List<CircleData> m_CircleDatas = new List<CircleData>();
//         [JsonProperty]
//         private readonly List<HollowConeData> m_HollowConeDatas = new List<HollowConeData>();
//         [JsonProperty]
//         private readonly List<HollowCylinderData> m_HollowCylinderDatas = new List<HollowCylinderData>();
//         [JsonProperty]
//         private readonly List<SphereData> m_SphereDatas = new List<SphereData>();
//         
//         private readonly SolidsOfRevolutionUniquenessValidators m_UniquenessValidators = new SolidsOfRevolutionUniquenessValidators();
//
//         private IShapeViewFactory m_ShapeViewFactory;
//
//         public IReadOnlyCollection<ShapeData> AllDatas =>
//             m_CircleDatas.Cast<ShapeData>()
//                 .Concat(m_HollowConeDatas)
//                 .Concat(m_HollowCylinderDatas)
//                 .Concat(m_SphereDatas)
//                 .ToList();
//
//         [OnDeserialized, UsedImplicitly]
//         private void OnDeserialized(StreamingContext context)
//         {
//             foreach (ShapeData shapeData in AllDatas)
//             {
//                 ProcessNewShapeData(shapeData);
//             }
//         }
//
//         public IReadOnlyList<TShapeData> GetShapeDatasList<TShapeData>() where TShapeData : ShapeData
//         {
//             return AllDatas.Where(data => data is TShapeData).Cast<TShapeData>().ToList();
//         }
//
//         public void Clear()
//         {
//             foreach (ShapeData shapeData in AllDatas)
//             {
//                 RemoveShapeData(shapeData);
//             }
//         }
//
//         public void SetViewFactory(IShapeViewFactory shapeViewFactory)
//         {
//             if (m_ShapeViewFactory != null)
//             {
//                 m_ShapeViewFactory.Dispose();
//             }
//             m_ShapeViewFactory = shapeViewFactory;
//             if (m_ShapeViewFactory == null)
//             {
//                 return;
//             }
//             foreach (ShapeData shapeData in AllDatas)
//             {
//                 shapeData.AttachView(m_ShapeViewFactory.RequestShapeView(shapeData));
//             }
//         }
//         
//         public CircleData CreateCircleData()
//         {
//             CircleData circleData = new CircleData();
//             m_CircleDatas.Add(circleData);
//             ProcessNewShapeData(circleData);
//             return circleData;
//         }
//
//         public HollowConeData CreateHollowConeData()
//         {
//             HollowConeData hollowConeData = new HollowConeData();
//             m_HollowConeDatas.Add(hollowConeData);
//             ProcessNewShapeData(hollowConeData);
//             return hollowConeData;
//         }
//
//         public HollowCylinderData CreateHollowCylinderData()
//         {
//             HollowCylinderData hollowCylinderData = new HollowCylinderData();
//             m_HollowCylinderDatas.Add(hollowCylinderData);
//             ProcessNewShapeData(hollowCylinderData);
//             return hollowCylinderData;
//         }
//
//         public CompositeShapeData CreateCompositeShapeData()
//         {
//             CompositeShapeData compositeShapeData = new CompositeShapeData();
//             m_CompositeShapeDatas.Add(compositeShapeData);
//             ProcessNewShapeData(compositeShapeData);
//             return compositeShapeData;
//         }
//
//         private void ProcessNewShapeData(ShapeData shapeData)
//         {
//             OnBecameDirty();
//             shapeData.NameUpdated += OnBecameDirty;
//             shapeData.GeometryUpdated += OnBecameDirty;
//             m_UniquenessValidators.AddShapeData(shapeData);
//             IShapeView view = m_ShapeViewFactory?.RequestShapeView(shapeData);
//             if (view != null)
//             {
//                 shapeData.AttachView(view);
//             }
//         }
//
//         public void RemoveShapeData(ShapeData shapeData)
//         {
//             m_UniquenessValidators.RemoveShapeData(shapeData);
//             switch (shapeData)
//             {
//                 case PointData pointData:
//                     m_PointDatas.Remove(pointData);
//                     break;
//                 case LineData lineData:
//                     m_LinetDatas.Remove(lineData);
//                     break;
//                 case PolygonData polygonData:
//                     m_PolygonDatas.Remove(polygonData);
//                     break;
//                 case CompositeShapeData compositeShapeData:
//                     m_CompositeShapeDatas.Remove(compositeShapeData);
//                     break;
//             }
//             if (shapeData.View != null)
//             {
//                 m_ShapeViewFactory?.ReleaseView(shapeData.View);
//             }
//             shapeData.DestroyData(); 
//             OnBecameDirty();
//         }
//
//         public void SetOrigin(Vector3 origin)
//         {
//             m_Origin = origin;
//             OnBecameDirty();
//         }
//
//         private void OnBecameDirty()
//         {
//             BecameDirty?.Invoke();
//         }
//     }
// }