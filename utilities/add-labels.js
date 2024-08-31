// From https://github.com/Relequestual/sensible-github-labels/blob/master/label-me.js

var gitLabel = require("git-label");

var config = {
  api: "https://api.github.com",
  repo: "Demetrioz/Agrigate",
  token: "xxx",
};

var labels = [
  {
    name: "Priority: Low",
    description:
      "This issue can probably be picked up by anyone looking to contribute to the project, as an entry fix",
    color: "#009800",
  },
  {
    name: "Priority: Medium",
    description: "his issue may be useful, and needs some attention.",
    color: "#fbca04",
  },
  {
    name: "Priority: High",
    description:
      "After critical issues are fixed, these should be dealt with before any further issues.",
    color: "#eb6420",
  },
  {
    name: "Priority: Critical",
    description:
      "This should be dealt with ASAP. Not fixing this issue would be a serious error.",
    color: "#e11d21",
  },
  {
    name: "Status: Abandoned",
    description:
      "It's believed that this issue is no longer important to the requestor and no one else has shown an i",
    color: "#000000",
  },
  {
    name: "Status: Accepted",
    desscription:
      "It's clear what the subject of the issue is about, and what the resolution should be.",
    color: "#009800",
  },
  {
    name: "Status: Available",
    description:
      "No one has claimed responsibility for resolving this issue. Generally this will be applied to bugs a",
    color: "#bfe5bf",
  },
  {
    name: "Status: Blocked",
    description:
      "There is another issue that needs to be resolved first, or a specific person is required to comment",
    color: "#e11d21",
  },
  {
    name: "Status: Completed",
    description:
      "Nothing further to be done with this issue. Awaiting to be closed by the requestor out of politeness",
    color: "#006b75",
  },
  {
    name: "Status: In Progress",
    description: "This issue is being worked on, and has someone assigned.",
    color: "#cccccc",
  },
  {
    name: "Status: On Hold",
    description:
      "Similar to blocked, but is assigned to someone. May also be assigned to someone because of their exp",
    color: "#e11d21",
  },
  {
    name: "Status: Review Needed",
    description:
      "The issue has a PR attached to it which needs to be reviewed. Should receive review by others in the",
    color: "#fbca04",
  },
  {
    name: "Status: Revision Needed",
    description:
      "At least two people have seen issues in the PR that makes them uneasy. Submitter of PR needs to revi",
    color: "#e11d21",
  },
  {
    name: "Type: Bug",
    description:
      "Inconsistencies or issues which will cause an issue or problem for users or implementors.",
    color: "#e11d21",
  },
  {
    name: "Type: Maintenance",
    description:
      "Updating phrasing or wording to make things clearer or removing ambiguity, without changing the func",
    color: "#fbca04",
  },
  {
    name: "Type: Enhancement",
    description:
      "Most issues will probably ask for additions or changes. It's expected that this type of issue will r",
    color: "#84b6eb",
  },
  {
    name: "Type: Question",
    description:
      "A query or seeking clarification on parts of the spec. Probably doesn't need the attention of everyo",
    color: "#cc317c",
  },
  {
    name: "Type: Spike",
    description:
      "An investigation required prior to beginning work on another issue",
    color: "#0052CC",
  },
];

// add specified labels from a repo
gitLabel
  .add(config, labels)
  .then(console.log) //=> success message
  .catch(console.log); //=> error message
