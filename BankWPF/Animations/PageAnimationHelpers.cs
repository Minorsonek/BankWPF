using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;

namespace BankWPF
{
    /// <summary>
    /// Helpers to animate pages in specific ways
    /// </summary>
    public static class PageAnimationHelpers
    {
        /// <summary>
        /// Slides the page in from the right
        /// </summary>
        /// <param name="page">The page to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromRight(this Page page, float seconds)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Add slide from right animation
            sb.AddSlideFromRight(seconds, page.WindowWidth);

            // Add fade in animation
            sb.AddFadeIn(seconds);

            // Start animating
            sb.Begin(page);

            // Make the page visible
            page.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)seconds * 1000);
        }

        /// <summary>
        /// Slides the page out to the left
        /// </summary>
        /// <param name="page">The page to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToLeft(this Page page, float seconds)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Add slide to left animation
            sb.AddSlideToLeft(seconds, page.WindowWidth);

            // Add fade out animation
            sb.AddFadeOut(seconds);

            // Start animating
            sb.Begin(page);

            // Hide the page
            page.Visibility = Visibility.Collapsed;

            // Wait for it to finish
            await Task.Delay((int)seconds * 1000);
        }
    }
}
