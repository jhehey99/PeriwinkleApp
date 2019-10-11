using PeriwinkleApp.Android.Source.Presenters.ClientFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;
using PeriwinkleApp.Core.Sources.Services;
using PeriwinkleApp.Core.Sources.Services.Interfaces;
using PeriwinkleApp.Core.Sources.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace PeriwinkleApp.Android.Source.Presenters.ClientPresenters
{
	public interface IClientViewResponsePresenter
	{
		Task LoadResponse(Mbes mbes);

	}
	public class ClientViewResponsePresenter : IClientViewResponsePresenter
	{
		private readonly IClientViewResponseView view;
		private readonly IMbesService mbesService;
		private MbesResponse response;

		public ClientViewResponsePresenter(IClientViewResponseView view)
		{
			this.view = view;
			mbesService = mbesService ?? new MbesService();
		}

		public async Task LoadResponse(Mbes mbes)
		{
			response = await mbesService.GetResponseByMbesId(mbes.MbesId.Value);
			Logger.Debug(response);

			/*
			response = new MbesResponse()
			{
				ScaleValues = new System.Collections.Generic.List<int>() {
				0, 1, 2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20
				}
			};
			*/

			// get average scores
			Score score = new Score()
			{
				BingeScore = GetBingeEatingAverageScore(),
				BulimiaScore = GetBulimiaNervosaAverageScore(),
				AnorexiaScore = GetAnorexiaNervosaAverageScore()
			};

			// display the response with the scores
			view.DisplayResponse(response, score);
		}

		public double GetBingeEatingAverageScore()
		{
			int[] scaleItems = { 1, 7, 8, 10, 14, 15, 18 };
			//double ave = scaleItems.Average();

			double average = 0;
			foreach(var i in scaleItems)
				average += response.ScaleValues.ElementAt(i);
			average /= scaleItems.Length;
			return average;
		}

		public double GetBulimiaNervosaAverageScore()
		{
			int[] scaleItems = { 2,3,11,12,20 };
			//double ave = scaleItems.Average();

			double average = 0;
			foreach (var i in scaleItems)
				average += response.ScaleValues.ElementAt(i);
			average /= scaleItems.Length;
			return average;
		}
		public double GetAnorexiaNervosaAverageScore()
		{
			int[] scaleItems = { 0, 4, 5, 6, 9, 13, 16, 17, 19 };
			//double ave = scaleItems.Average();

			double average = 0;
			foreach (var i in scaleItems)
				average += response.ScaleValues.ElementAt(i);
			average /= scaleItems.Length;
			return average;
		}
	}

	public class Score
	{
		public double BingeScore { get; set; }
		public double BulimiaScore { get; set; }
		public double AnorexiaScore { get; set; }
	}
}