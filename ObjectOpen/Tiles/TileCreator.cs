using System.Drawing;

namespace Tiles
{
    public class TileCreator
    {
        private Random _rnd;
        private float _minSizeCoefficient;

        public TileCreator(int seed, float minSizeCoeff)
        {
            _rnd = new Random(seed);
            _minSizeCoefficient = minSizeCoeff;
        }

        public List<RectangleF> SubdivideIntoNineTiles(RectangleF baseRectangle)
        {
            float tileMinSizeNormalised = 1f / 3f * _minSizeCoefficient;

            float verticalRndSum = 1f - tileMinSizeNormalised * 3;
            float[] verticalSteps = CalculateSteps(tileMinSizeNormalised, verticalRndSum, 3, baseRectangle.Height);

            float horizontalRndSum = 1f - tileMinSizeNormalised * 3;
            float[] horizontalTopSteps = CalculateSteps(tileMinSizeNormalised, horizontalRndSum, 3, baseRectangle.Width);
            float[] horizontalCenterSteps = CalculateSteps(tileMinSizeNormalised, horizontalRndSum, 3, baseRectangle.Width);
            float[] horizontalBottomSteps = CalculateSteps(tileMinSizeNormalised, horizontalRndSum, 3, baseRectangle.Width);

            List<RectangleF> tiles = new(9);
            PointF currentPoint = baseRectangle.Location;

            tiles.AddRange(CalculateRectangles(horizontalTopSteps, verticalSteps[0], currentPoint));
            currentPoint.Y += verticalSteps[0];

            tiles.AddRange(CalculateRectangles(horizontalCenterSteps, verticalSteps[1], currentPoint));
            currentPoint.Y += verticalSteps[1];

            tiles.AddRange(CalculateRectangles(horizontalBottomSteps, verticalSteps[2], currentPoint));

            return tiles;
        }

        private List<RectangleF> CalculateRectangles(float[] horizontalSteps, float verticalStep, PointF currentPoint)
        {
            List<RectangleF> rectangles = new(horizontalSteps.Length);
            foreach (var horizontalStep in horizontalSteps)
            {
                rectangles.Add(new RectangleF(currentPoint, new SizeF(horizontalStep, verticalStep)));
                currentPoint.X += horizontalStep;
            }

            return rectangles;
        }

        private float[] CalculateSteps(float biosValue, float rndSum, int count, float scaleCoefficient)
        {
            float[] rndValues = new float[count];
            for (int i = 0; i < rndValues.Length; i++)
                rndValues[i] = (float)_rnd.NextDouble();

            float coeff = rndSum / rndValues.Sum();
            for (int i = 0; i < rndValues.Length; i++)
                rndValues[i] = (rndValues[i] * coeff + biosValue) * scaleCoefficient;

            return rndValues;
        }
    }
}
