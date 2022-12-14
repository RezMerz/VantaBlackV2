////////////////////////////////////////////////////////////////////////////////
//  
// @module Affter Effect Importer
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Xml;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AEDataParcer  {

	private static float frameDuration;

	public static AEAnimationData ParceAnimationData(string data) {

		XmlDocument doc =  new XmlDocument();
		doc.LoadXml(data);

		AEAnimationData animation = new AEAnimationData ();

		XmlNode anim = doc.SelectSingleNode("after_affect_animation_doc");


		XmlNode meta = anim.SelectSingleNode("meta");
		animation.frameDuration = GetFloat (meta, "frameDuration");
		animation.totalFrames = GetInt (meta, "totalFrames");
		animation.duration =  GetFloat (meta, "duration");

		frameDuration = animation.frameDuration;


		XmlNode composition = anim.SelectSingleNode("composition");
		animation.composition = ParseComposition (composition);




		XmlNode sub_items = anim.SelectSingleNode("sub_items");
		XmlNodeList usedCompositions = sub_items.SelectNodes("composition");

		foreach (XmlNode c in usedCompositions) {
			animation.addComposition(ParseComposition (c));
		}



		return animation;
	}


	public static AECompositionTemplate ParseComposition(XmlNode composition) {

		AECompositionTemplate comp = new AECompositionTemplate ();

		comp.id = System.Convert.ToInt32(composition.Attributes.GetNamedItem("id").Value);
		comp.width = System.Convert.ToSingle(composition.Attributes.GetNamedItem("w").Value);
		comp.heigh = System.Convert.ToSingle(composition.Attributes.GetNamedItem("h").Value);


		XmlNode meta = composition.SelectSingleNode ("meta");
		comp.duration = GetFloat (meta, "duration");
		comp.totalFrames = GetInt (meta, "totalFrames");
		comp.frameDuration = frameDuration;


		XmlNodeList layersNodes = composition.SelectNodes("layer");

		foreach (XmlNode layerNode in layersNodes) {
			AELayerTemplate layer = new AELayerTemplate ();

			string layerType = layerNode.Attributes.GetNamedItem("type").Value;

			if(layerType.Equals("Composition")) {
				layer.type = AELayerType.COMPOSITION;
				layer.id  = System.Convert.ToInt32(layerNode.Attributes.GetNamedItem("id").Value);
			}

			if(layerType.Equals("Footage")) {
				layer.type = AELayerType.FOOTAGE;
				layer.source = layerNode.Attributes.GetNamedItem("source").Value;
			}




			layer.index  = Int32.Parse (layerNode.Attributes.GetNamedItem("index").Value);

			if(layerNode.Attributes.GetNamedItem("parent").Value != "none") {
				layer.parent = Int32.Parse (layerNode.Attributes.GetNamedItem("parent").Value);
			} else {
				layer.parent = 0;
			}


			layer.width  = System.Convert.ToInt32(layerNode.Attributes.GetNamedItem("w").Value);
			layer.height = System.Convert.ToInt32(layerNode.Attributes.GetNamedItem("h").Value);

			layer.name = layerNode.Attributes.GetNamedItem("name").Value;


			string blendMode = layerNode.Attributes.GetNamedItem("blending").Value;
			layer.blending = (AELayerBlendingType) System.Enum.Parse (typeof(AELayerBlendingType), blendMode);

			float inTime  = GetFloat (layerNode, "inPoint");
			float outTime = GetFloat (layerNode, "outPoint");
			layer.setInOutTime (inTime, outTime, comp);


			XmlNodeList frameNodes = layerNode.SelectNodes("keyframe");

			AEFrameTemplate lastFrame = null;
			foreach (XmlNode frameNode in frameNodes) {
				AEFrameTemplate frame = new AEFrameTemplate ();
				frame.index = Int32.Parse (frameNode.Attributes.GetNamedItem("frame").Value);
				//frame.time = System.Convert.ToSingle (frameNode.Attributes.GetNamedItem("time").Value);

				XmlNode source = frameNode.SelectSingleNode ("source");
				XmlNodeList propertyNodes = source.SelectNodes ("property");
				foreach(XmlNode propertyNode in propertyNodes) {
					string propType = propertyNode.Attributes.GetNamedItem ("name").Value;

					switch(propType) {
						case AEPropertyType.ANCHOR_POINT:
						frame.pivot = new Vector3 (GetFloat(propertyNode, "x"), GetFloat (propertyNode, "y"), GetFloat (propertyNode, "z"));
						break;
						case AEPropertyType.POSITION:
						frame.SetPosition( new Vector3 (GetFloat(propertyNode, "x"), GetFloat (propertyNode, "y"), GetFloat (propertyNode, "z")) );
						break;
						case AEPropertyType.SCALE:
						frame.scale = new Vector3 (GetFloat(propertyNode, "x"), GetFloat (propertyNode, "y"), GetFloat (propertyNode, "z")) * 0.01f;
						break;
						case AEPropertyType.ROTATION:
						frame.rotation = GetFloat (propertyNode, "val");
						break;
						case AEPropertyType.OPACITY:
						frame.opacity = GetFloat (propertyNode, "val");
						break;
					}
				}

				frame.CompareToFrame (lastFrame);

				lastFrame = frame;
				layer.addFrame (frame);

			}

			comp.addLayer (layer);
		}

		return comp;
	}


	public static float GetFloat(XmlNode node, string name) {
		return System.Convert.ToSingle(node.Attributes.GetNamedItem(name).Value);
	}

	public static int GetInt(XmlNode node, string name) {
		return System.Convert.ToInt32(node.Attributes.GetNamedItem(name).Value);
	}
}
