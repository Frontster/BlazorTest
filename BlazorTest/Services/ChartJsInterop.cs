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
            return await _jsRuntime.InvokeAsync<bool>("renderChart", canvasId, labels, data, title);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error rendering chart: {ex.Message}");
            return false;
        }
    }
}