var sampleSVG = d3.select("#viz")
    .append("svg")
    .attr("width", 100)
    .attr("height", 100);

sampleSVG.append("circle")
    .style("stroke", "gray")
    .style("fill", "white")
    .attr("r", 40)
    .attr("cx", 50)
    .attr("cy", 50)
    .transition()
    .delay(100)
    .duration(1000)
    .attr("r", 10)
    .attr("cx", 30)
    .style("fill", "black");