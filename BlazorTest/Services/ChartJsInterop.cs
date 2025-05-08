using Microsoft.JSInterop;

namespace BlazorTest.Services;

/// <summary>
/// Service for JavaScript interop related to Chart.js
/// Handles rendering charts and other JavaScript functionality
/// </summary>
public class ChartJsInterop
{
    private readonly IJSRuntime _jsRuntime;

    public ChartJsInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        Console.WriteLine("ChartJsInterop: Constructor called");
    }

    /// <summary>
    /// Renders a chart using Chart.js
    /// </summary>
    /// <param name="canvasId">The ID of the canvas element to render the chart on</param>
    /// <param name="labels">The labels for the chart (e.g., months)</param>
    /// <param name="data">The data for the chart (e.g., revenue values)</param>
    /// <param name="title">The title for the chart</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> RenderChartAsync(string canvasId, IEnumerable<string> labels, IEnumerable<decimal> data, string title)
    {
        try
        {
            Console.WriteLine($"ChartJsInterop: Rendering chart on canvas ID '{canvasId}'");

            // Convert decimal values to double for JavaScript
            var jsData = data.Select(d => (double)d).ToArray();

            return await _jsRuntime.InvokeAsync<bool>("renderChart", canvasId, labels.ToArray(), jsData, title);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"ChartJsInterop: Error rendering chart: {ex.Message}");
            return false;
        }
    }
}