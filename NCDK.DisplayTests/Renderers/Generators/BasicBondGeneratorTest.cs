using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCDK.Renderers.Elements;
using System.Windows;

namespace NCDK.Renderers.Generators
{
    /// <summary>
    /// Test the <see cref="BasicBondGenerator"/>.
    /// </summary>
    // @author     maclean
    // @cdk.module test-renderbasic
    [TestClass()]
    public class BasicBondGeneratorTest : AbstractGeneratorTest<IAtomContainer>
    {
        private BasicBondGenerator generator;

        static IRenderingElement Unbox(IRenderingElement element)
        {
            if (element is MarkedElement)
                return ((MarkedElement)element).Element();
            return element;
        }

        public override Rect GetCustomCanvas()
        {
            return Rect.Empty;
        }

        public BasicBondGeneratorTest()
            : base()
        {
            this.generator = new BasicBondGenerator();
            base.SetTestedGenerator(generator);
        }

        [TestMethod()]
        public void TestSingleAtom()
        {
            var singleAtom = MakeSingleAtom();

            // nothing should be made
            var root = generator.Generate(singleAtom, model);
            var elements = elementUtil.GetAllSimpleElements(root);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod()]
        public void TestSingleBond()
        {
            var container = MakeSingleBond();

            // generate the single line element
            var root = generator.Generate(container, model);
            var elements = elementUtil.GetAllSimpleElements(root);
            Assert.AreEqual(1, elements.Count);

            // test that the endpoints are distinct
            var line = (LineElement)elements[0];
            Assert.AreNotSame(0, AbstractGeneratorTest<IAtomContainer>.Length(line));
        }

        [TestMethod()]
        public void TestSquare()
        {
            var square = MakeSquare();

            // generate all four bonds
            var root = generator.Generate(square, model);
            var elements = elementUtil.GetAllSimpleElements(root);
            Assert.AreEqual(4, elements.Count);

            // test that the center is at the origin
            Assert.AreEqual(new Point(0, 0), Center(elements));
        }
    }
}
