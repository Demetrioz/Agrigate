// From https://github.com/Relequestual/sensible-github-labels/blob/master/label-me.js

var gitLabel = require("git-label");

var config = {
  api: "https://api.github.com",
  repo: "Demetrioz/Agrigate",
  token: "xxx",
};

var labels = [
  { name: "Priority: Low", color: "#009800" },
  { name: "Priority: Medium", color: "#fbca04" },
  { name: "Priority: High", color: "#eb6420" },
  { name: "Priority: Critical", color: "#e11d21" },
  { name: "Status: Abandoned", color: "#000000" },
  { name: "Status: Accepted", color: "#009800" },
  { name: "Status: Available", color: "#bfe5bf" },
  { name: "Status: Blocked", color: "#e11d21" },
  { name: "Status: Completed", color: "#006b75" },
  { name: "Status: In Progress", color: "#cccccc" },
  { name: "Status: On Hold", color: "#e11d21" },
  { name: "Status: Review Needed", color: "#fbca04" },
  { name: "Status: Revision Needed", color: "#e11d21" },
  { name: "Type: Bug", color: "#e11d21" },
  { name: "Type: Maintenance", color: "#fbca04" },
  { name: "Type: Enhancement", color: "#84b6eb" },
  { name: "Type: Question", color: "#cc317c" },
];

// add specified labels from a repo
gitLabel
  .add(config, labels)
  .then(console.log) //=> success message
  .catch(console.log); //=> error message
