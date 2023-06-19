import { useEffect, useState } from "react";
import CommonService from "src/_cores/_services/common.service";
import { WebDirectional } from "src/_shared/_components";

const BlogPage = () => {
  const [blogInfo, setBlogInfo] = useState<any>(null);

  useEffect(() => {
    getBlog();
  }, []);

  const getBlog = () => {
    const param = { id: 1, type: 'BLOG' };
    CommonService.getBlog(param).then(res => {
      if (res.data) {
        setBlogInfo(res.data);
      }
    }).catch(error => {
      console.log(error);
    });
  }

  return (
    <div className="custom-container">
      <div className="content__wrapper products__content-wrapper">
        <div className="content__inner w-full">
          <WebDirectional items={[
            { path: '/', name: 'BlogTitle' }
          ]} />
          <div className="blog-content">
            {blogInfo && blogInfo.blogContent ? (
              <div dangerouslySetInnerHTML={{__html: blogInfo.blogContent}}></div>
            ) : null}
          </div>
        </div>
      </div>
    </div>
  );
};

export default BlogPage;
