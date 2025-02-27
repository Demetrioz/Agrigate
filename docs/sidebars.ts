import type { SidebarsConfig } from "@docusaurus/plugin-content-docs";

/**
 * Creating a sidebar enables you to:
 - create an ordered group of docs
 - render a sidebar for each doc of that group
 - provide next/previous navigation

 The sidebars can be generated from the filesystem, or explicitly defined here.

 Create as many sidebars as you want.
 */
const sidebars: SidebarsConfig = {
  // By default, Docusaurus generates a sidebar from the docs folder structure
  docs: [
    "intro",
    // {
    //   type: "category",
    //   label: "Getting Started",
    //   link: {
    //     type: "generated-index",
    //   },
    //   items: [
    //     "getting-started/client",
    //     "getting-started/server",
    //     "getting-started/devices",
    //   ],
    // },
  ],
  guides: ["guides/intro"],
  // devices: [
  //   "devices/intro",
  //   {
  //     type: "category",
  //     label: "Raised Bed Irrigation",
  //     link: {
  //       type: "generated-index",
  //     },
  //     items: ["devices/raised-bed-irrigation/intro"],
  //   },
  //   {
  //     type: "category",
  //     label: "Greenhouse Controller",
  //     link: {
  //       type: "generated-index",
  //     },
  //     items: ["devices/greenhouse-controller/intro"],
  //   },
  //   {
  //     type: "category",
  //     label: "Grow Tent Controller",
  //     link: {
  //       type: "generated-index",
  //     },
  //     items: [
  //       "devices/grow-tent-controller/intro",
  //       "devices/grow-tent-controller/bom",
  //     ],
  //   },
  // ],
  technical: [
    "technical/intro", 
    "technical/setup", 
    // "technical/rules"
  ],
  releases: [
    "releases/intro",
    // {
    //   type: "category",
    //   label: "Api",
    //   link: {
    //     type: "generated-index",
    //   },
    //   items: [
    //     "releases/api/0.1.0",
    //     "releases/api/0.2.0",
    //     "releases/api/0.3.0",
    //     "releases/api/0.4.0",
    //   ],
    // },
    // {
    //   type: "category",
    //   label: "Client",
    //   link: {
    //     type: "generated-index",
    //   },
    //   items: ["releases/client/0.1.0", "releases/client/0.2.0"],
    // },
    // {
    //   type: "category",
    //   label: "EventService",
    //   link: {
    //     type: "generated-index",
    //   },
    //   items: [
    //     "releases/eventservice/0.1.0",
    //     "releases/eventservice/0.2.0",
    //     "releases/eventservice/0.3.0",
    //   ],
    // },
  ],

  // But you can create a sidebar manually
  /*
  tutorialSidebar: [
    'intro',
    'hello',
    {
      type: 'category',
      label: 'Tutorial',
      items: ['tutorial-basics/create-a-document'],
    },
  ],
   */
};

export default sidebars;
