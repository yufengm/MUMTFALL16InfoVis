var sampleSVG = d3.select("#viz")
    .append("svg")
    .attr("width", window.innerWidth)
    .attr("height", window.innerHeight);

sampleSVG.append("text")
    .text("Cloud")
    .style("fill", "white")
    .style("font-size", "5px")
    .attr("x", "100")
    .attr("y", "100")
    .transition()
    .delay(100)
    .duration(3000)
    .style("font-size", "72px")
    .style("fill", "red");