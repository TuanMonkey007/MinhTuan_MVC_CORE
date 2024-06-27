// src/helpers/CustomEditor.js
import ClassicEditorBase from '@ckeditor/ckeditor5-editor-classic/src/classiceditor';
import Essentials from '@ckeditor/ckeditor5-essentials/src/essentials';
import UploadAdapter from '@ckeditor/ckeditor5-adapter-ckfinder/src/uploadadapter';
import Autoformat from '@ckeditor/ckeditor5-autoformat/src/autoformat';
import Bold from '@ckeditor/ckeditor5-basic-styles/src/bold';
import Italic from '@ckeditor/ckeditor5-basic-styles/src/italic';
import BlockQuote from '@ckeditor/ckeditor5-block-quote/src/blockquote';
import Heading from '@ckeditor/ckeditor5-heading/src/heading';
import Image from '@ckeditor/ckeditor5-image/src/image';
import ImageToolbar from '@ckeditor/ckeditor5-image/src/imagetoolbar';
import ImageStyle from '@ckeditor/ckeditor5-image/src/imagestyle';
import ImageResize from '@ckeditor/ckeditor5-image/src/imageresize';
import Link from '@ckeditor/ckeditor5-link/src/link';
import List from '@ckeditor/ckeditor5-list/src/list';
import Paragraph from '@ckeditor/ckeditor5-paragraph/src/paragraph';
import MyBase64UploadAdapterPlugin from '@/helpers/UploadAdapter';

export default class CustomEditor extends ClassicEditorBase {}

CustomEditor.builtinPlugins = [
  Essentials,
  UploadAdapter,
  Autoformat,
  Bold,
  Italic,
  BlockQuote,
  Heading,
  Image,
  ImageToolbar,
  ImageStyle,
  ImageResize,
  Link,
  List,
  Paragraph,
  MyBase64UploadAdapterPlugin
];

CustomEditor.defaultConfig = {
  toolbar: {
    items: [
      'heading', '|',
      'bold', 'italic', 'link', 'bulletedList', 'numberedList', 'blockQuote', '|',
      'uploadImage', 'insertTable', 'tableColumn', 'tableRow', 'mergeTableCells', '|',
      'undo', 'redo', 'alignment', 'fontSize', 'fontFamily', 'fontColor', 'fontBackgroundColor'
    ]
  },
  image: {
    toolbar: [
      'imageTextAlternative', 'imageStyle:full', 'imageStyle:side', '|', 'resizeImage'
    ],
    resizeOptions: [
      {
        name: 'resizeImage:original',
        value: null,
        icon: 'original'
      },
      {
        name: 'resizeImage:50',
        value: '50',
        icon: 'medium'
      },
      {
        name: 'resizeImage:75',
        value: '75',
        icon: 'large'
      }
    ],
    resizeUnit: '%'
  },
  language: 'en'
};
