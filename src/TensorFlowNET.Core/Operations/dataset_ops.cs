﻿using System;
using System.Collections.Generic;
using System.Text;
using static Tensorflow.Binding;

namespace Tensorflow
{
    public class dataset_ops
    {
        /// <summary>
        /// Creates a dataset that emits each dim-0 slice of `components` once.
        /// </summary>
        /// <param name="components"></param>
        /// <param name="output_shapes"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Tensor tensor_slice_dataset(Tensor[] components, TensorShape[] output_shapes, string name = null)
        {
            if (tf.context.executing_eagerly())
            {
                var results = tf.Runner.TFE_FastPathExecute(tf.context, tf.context.device_name,
                    "TensorSliceDataset", name,
                    null,
                    new object[] 
                    { 
                        components,
                        "output_shapes", output_shapes
                    });
                return results[0];
            }

            throw new NotImplementedException("");
        }

        public Tensor repeat_dataset(Tensor input_dataset, Tensor count, TF_DataType[] output_types, TensorShape[] output_shapes, string name = null)
        {
            if (tf.context.executing_eagerly())
            {
                var results = tf.Runner.TFE_FastPathExecute(tf.context, tf.context.device_name,
                    "RepeatDataset", name,
                    null,
                    input_dataset, count,
                    "output_types", output_types,
                    "output_shapes", output_shapes);
                return results[0];
            }

            throw new NotImplementedException("");
        }

        public Tensor shuffle_dataset_v3(Tensor input_dataset, Tensor buffer_size, 
            Tensor seed, Tensor seed2, Tensor seed_generator,
            TF_DataType[] output_types, TensorShape[] output_shapes, 
            bool reshuffle_each_iteration = true,
            string name = null)
        {
            if (tf.context.executing_eagerly())
            {
                var results = tf.Runner.TFE_FastPathExecute(tf.context, tf.context.device_name,
                    "ShuffleDatasetV3", name,
                    null,
                    input_dataset, buffer_size,
                    seed, seed2, seed_generator,
                    "reshuffle_each_iteration", reshuffle_each_iteration,
                    "output_types", output_types,
                    "output_shapes", output_shapes);
                return results[0];
            }

            throw new NotImplementedException("");
        }

        public Tensor dummy_seed_generator(string name = null)
        {
            if (tf.context.executing_eagerly())
            {
                var results = tf.Runner.TFE_FastPathExecute(tf.context, tf.context.device_name,
                    "DummySeedGenerator", name,
                    null);
                return results[0];
            }

            throw new NotImplementedException("");
        }

        /// <summary>
        /// Creates a dataset that batches `batch_size` elements from `input_dataset`.
        /// </summary>
        /// <param name="input_dataset"></param>
        /// <param name="buffer_size"></param>
        /// <param name="drop_remainder"></param>
        /// <param name="output_types"></param>
        /// <param name="output_shapes"></param>
        /// <param name="parallel_copy"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Tensor batch_dataset_v2(Tensor input_dataset, Tensor buffer_size,
            Tensor drop_remainder,
            TF_DataType[] output_types, TensorShape[] output_shapes,
            bool parallel_copy = false,
            string name = null)
        {
            if (tf.context.executing_eagerly())
            {
                var results = tf.Runner.TFE_FastPathExecute(tf.context, tf.context.device_name,
                    "BatchDatasetV2", name,
                    null,
                    input_dataset, buffer_size, drop_remainder,
                    "parallel_copy", parallel_copy,
                    "output_types", output_types,
                    "output_shapes", output_shapes);
                return results[0];
            }

            throw new NotImplementedException("");
        }

        /// <summary>
        /// Creates a dataset that asynchronously prefetches elements from `input_dataset`.
        /// </summary>
        /// <param name="input_dataset"></param>
        /// <param name="buffer_size"></param>
        /// <param name="output_types"></param>
        /// <param name="output_shapes"></param>
        /// <param name="slack_period"></param>
        /// <param name="legacy_autotune"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Tensor prefetch_dataset(Tensor input_dataset, Tensor buffer_size,
            TF_DataType[] output_types, TensorShape[] output_shapes,
            int? slack_period = 0,
            bool legacy_autotune = true,
            string name = null)
        {
            if (tf.context.executing_eagerly())
            {
                var results = tf.Runner.TFE_FastPathExecute(tf.context, tf.context.device_name,
                    "PrefetchDataset", name,
                    null,
                    input_dataset, buffer_size, 
                    "output_types", output_types,
                    "output_shapes", output_shapes,
                    "slack_period", slack_period, 
                    "legacy_autotune", legacy_autotune);
                return results[0];
            }

            throw new NotImplementedException("");
        }

        /// <summary>
        /// Creates a dataset that contains `count` elements from the `input_dataset`.
        /// </summary>
        /// <param name="input_dataset"></param>
        /// <param name="count"></param>
        /// <param name="output_types"></param>
        /// <param name="output_shapes"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Tensor take_dataset(Tensor input_dataset, Tensor count,
            TF_DataType[] output_types, TensorShape[] output_shapes,
            string name = null)
        {
            if (tf.context.executing_eagerly())
            {
                var results = tf.Runner.TFE_FastPathExecute(tf.context, tf.context.device_name,
                    "TakeDataset", name,
                    null,
                    input_dataset, count,
                    "output_types", output_types,
                    "output_shapes", output_shapes);
                return results[0];
            }

            throw new NotImplementedException("");
        }
    }
}
