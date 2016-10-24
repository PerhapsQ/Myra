﻿using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using NLog;

namespace Myra.UIEditor
{
	public class State
	{
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public const string StateFileName = "Myra.UIEditor.config";

		public static string StateFilePath
		{
			get
			{
				var result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), StateFileName);
				return result;
			}
		}

		public Point Size { get; set; }
		public float TopSplitterPosition { get; set; }
		public float RightSplitterPosition { get; set; }
		public string EditedFile { get; set; }
		public int[] CustomColors { get; set; }

		public void Save()
		{
			_logger.Info("Saving state: {0}", ToString());

			using (var stream = new StreamWriter(StateFilePath, false))
			{
				var serializer = new XmlSerializer(typeof (State));
				serializer.Serialize(stream, this);
			}
		}

		public static State Load()
		{
			_logger.Info("Restoring state.");

			if (!File.Exists(StateFilePath))
			{
				_logger.Info("State file '{0}' doesnt exit.", StateFilePath);

				return null;
			}

			State state;
			using (var stream = new StreamReader(StateFilePath))
			{
				var serializer = new XmlSerializer(typeof (State));
				state = (State) serializer.Deserialize(stream);
			}

			_logger.Info("Result: {0}", state);

			return state;
		}

		public override string ToString()
		{
			return string.Format("Size = {0}\n" +
			                     "TopSplitter = {1:0.##}\n" +
			                     "RightSplitter= {2:0.##}\n" +
			                     "EditedFile = {3}\n" +
			                     "CustomColors = {4}",
				Size,
				TopSplitterPosition,
				RightSplitterPosition,
				EditedFile,
				CustomColors);
		}
	}
}