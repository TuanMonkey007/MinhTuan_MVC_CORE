class MyBase64UploadAdapter {
    constructor(loader) {
      this.loader = loader;
    }
  
    upload() {
      return this.loader.file
        .then(file => new Promise((resolve, reject) => {
          const reader = new FileReader();
          reader.readAsDataURL(file);
          reader.onload = () => {
            resolve({
              default: reader.result,
            });
          };
          reader.onerror = error => {
            reject(error);
          };
        }));
    }
  
    abort() {
      // Handle abort if needed
    }
  }
  
  function MyBase64UploadAdapterPlugin(editor) {
    editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
      return new MyBase64UploadAdapter(loader);
    };
  }
  
  export default MyBase64UploadAdapterPlugin;
  